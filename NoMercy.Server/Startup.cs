using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NoMercy.Database;
using NoMercy.Helpers;
using NoMercy.Server.Jobs;
using NoMercy.Server.Logic;

namespace NoMercy.Server
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<UserLogic>();
            services.AddSingleton<JobQueue>();
            services.AddDbContext<MediaContext>(options =>
            {
                options.UseSqlite($"Data Source={AppFiles.MediaDatabase}");
            });

            services.ConfigureHttpJsonOptions(config =>
            {
                config.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddCors();
            services.AddAuthorizationBuilder().AddPolicy("api", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
                policy.RequireClaim("scope", "api");
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://auth-dev.nomercy.tv/realms/NoMercyTV"; // your keycloak address
                    options.Audience = "nomercy-server";
                    options.RequireHttpsMetadata =
                        true; // For testing, you might want to set this to true in production
                });

            services.AddAuthorization();
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
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
                            AuthorizationUrl = new Uri("https://auth-dev.nomercy.tv/realms/NoMercyTV/protocol/openid-connect/auth"),
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
                            {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}},
                        new string[] { }
                    }
                });
            });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        
            app.UseHttpsRedirection();
        
            app.UseAuthentication();
            app.UseAuthorization();
        
            app.UseMvcWithDefaultRoute();
            
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NoMercy API");
                    options.RoutePrefix = string.Empty;
                });
            }
        }
    }
}