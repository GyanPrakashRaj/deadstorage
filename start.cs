using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using VueCliMiddleware;
using WowDash.Infrastructure;
using System.Reflection;
using System.IO;
using System;
using Microsoft.AspNetCore.Identity;

namespace WowDash.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            // Not currently working; for reimplementation later
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
            })
                .AddJwtBearer(options =>
                {
                    options.Authority = "webApi";
                    options.Audience = "https://localhost:5000/";
                });

            services.AddHttpClient();

            services.AddControllersWithViews();

            // In production, the Vue files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "WebUI/ClientApp/dist";
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The offending redirect
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            dbContext.Database.Migrate();

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                // Prevents database schema from appearing at the bottom of the docs
                //c.DefaultModelsExpandDepth(-1);
            });

            app.UseRouting();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //if (env.IsDevelopment())
                //{
                //   endpoints.MapToVueCliProxy(
                //   "{*path}",
                //   new Microsoft.AspNetCore.SpaServices.SpaOptions { SourcePath = "WebUI/ClientApp" },
                //   npmScript: "serve",
                //   regex: "Compiled Successfully");
                //}
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "WebUI/ClientApp";

                if (env.IsDevelopment())
                {
                //    // run npm process with client app
                        spa.UseVueCli(npmScript: "serve", port: 8080);
                //    // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead,
                //    // app should be already running before starting a .NET client:
                     // spa.UseProxyToSpaDevelopmentServer("http://localhost:8080"); // your Vue app port
                }
            });
        }
    }
}