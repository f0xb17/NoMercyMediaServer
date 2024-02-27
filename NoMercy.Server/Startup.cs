using System.Net;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Middleware;
using NoMercy.Server.system;

namespace NoMercy.Server
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton<JobQueue>();
            services.AddControllers().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                jsonOptions.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );

            services.AddDbContext<QueueContext>(optionsAction =>
            {
                optionsAction.UseSqlite($"Data Source={AppFiles.QueueDatabase}");
            });
            services.AddDbContext<MediaContext>(optionsAction =>
            {
                optionsAction.UseSqlite($"Data Source={AppFiles.MediaDatabase}");
            });

            services.ConfigureHttpJsonOptions(config =>
            {
                config.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddAuthorizationBuilder()
                .AddPolicy("api", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
                    policy.RequireClaim("scope", "openid", "profile");
                    policy.AddRequirements([
                        new AssertionRequirement(context =>
                        {
                            using MediaContext mediaContext = new();
                            User? user = mediaContext.Users
                                .FirstOrDefault(user =>
                                    user.Id == Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                                          string.Empty));
                            return user is not null;
                        })
                    ]);
                });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://auth-dev.nomercy.tv/realms/NoMercyTV";
                    options.RequireHttpsMetadata = true;
                    options.Audience = "nomercy-ui";
                    options.Audience = "nomercy-server";
                });
            services.AddAuthorization();

            services.AddCors();
            services.AddApiVersioning();
            services.AddDirectoryBrowser();

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NoMercy API",
                    Version = "v1",
                    Description = "NoMercy API",
                    Contact = new OpenApiContact
                    {
                        Name = "NoMercy",
                        Email = "stoney@nomercy.tv",
                        Url = new Uri("https://nomercy.tv")
                    }
                });
                options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl =
                                new Uri("https://auth-dev.nomercy.tv/realms/NoMercyTV/protocol/openid-connect/auth"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "openid" },
                                { "profile", "profile" }
                            },
                        }
                    }
                });

                // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                // {
                //     Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //     Name = "Authorization",
                //     In = ParameterLocation.Header,
                //     Type = SecuritySchemeType.Http,
                //     Scheme = "bearer",
                //     BearerFormat = "JWT"
                // });

                OpenApiSecurityScheme keycloakSecurityScheme = new()
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Keycloak",
                        Type = ReferenceType.SecurityScheme,
                    },
                    In = ParameterLocation.Header,
                    Name = "Bearer",
                    Scheme = "Bearer",
                };

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { keycloakSecurityScheme, Array.Empty<string>() },
                    {
                        new OpenApiSecurityScheme
                            { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        new string[] { }
                    }
                });
            });

            services.AddResponseCompression(options => { options.EnableForHttps = true; });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .WithHeaders("Access-Control-Allow-Private-Network", "true")
                    .AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseRequestLocalization();
            app.UseResponseCaching();
            app.UseWebSockets();

            app.UseMiddleware<TokenParamAuthMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NoMercy API");
                    options.RoutePrefix = string.Empty;
                    options.OAuthClientId("nomercy-server");
                    options.OAuthScopes("openid");
                    options.EnablePersistAuthorization();
                    options.EnableTryItOutByDefault();
                });
            }

            app.UseMvcWithDefaultRoute();

            app.UseWebSockets();

            MediaContext mediaContext = new();
            List<Folder> folderLibraries = mediaContext.Folders
                .ToList();

            foreach (var folder in folderLibraries)
            {
                if (!Directory.Exists(folder.Path)) continue;

                var path = app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(folder.Path),
                    RequestPath = new PathString($"/{folder.Id}"),
                    ServeUnknownFileTypes = true,
                    HttpsCompression = HttpsCompressionMode.Compress,
                    OnPrepareResponse = ctx =>
                    {
                        if (ctx.Context.User.Identity?.IsAuthenticated is false)
                        {
                            ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                    }
                });

                if (!env.IsDevelopment()) continue;

                path.UseDirectoryBrowser(new DirectoryBrowserOptions
                {
                    FileProvider = new PhysicalFileProvider(folder.Path),
                    RequestPath = new PathString($"/{folder.Id}"),
                });
            }
        }
    }
}