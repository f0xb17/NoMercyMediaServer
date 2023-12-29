using ElectronNET.API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using NoMercy.Server.Helpers;
using NoMercy.Server.Logic;

namespace NoMercy.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public static ApiInfo? ApiInfo { get; set; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton<UserLogic>();
            services.AddAuthorizationBuilder().AddPolicy("api", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
                policy.RequireClaim("scope", "api");
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://auth-dev2.nomercy.tv/realms/NoMercyTV"; // your keycloak address
                    options.Audience = "nomercy-server";
                    options.RequireHttpsMetadata = true; // For testing, you might want to set this to true in production
                });

            services.AddAuthorization();
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        
            if (!HybridSupport.IsElectronActive) return;
            await Task.Run(ElectronWindows.Start);
            
        }
    }
}