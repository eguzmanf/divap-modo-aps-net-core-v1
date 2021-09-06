using DotacionWEBCore.Data;
using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using DotacionWEBCore.Areas.Identity.Data;
using Vereyon.Web;
using DotacionWEBCore.Helpers.Rut.V1;
using DotacionWEBCore.Helpers.Rut.V2;
using DotacionWEBCore.Helpers.String.StringCase;
using DotacionWEBCore.Helpers.Perfil;

namespace DotacionWEBCore
{


    public class Startup
    {
        
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }
        


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // habilita protocolo de seguridad TLS 1.2
            if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }


            // Add framework services
            services.AddDataProtection();
            services.AddMvc();
            var defaultconnection = Configuration.GetConnectionString("DatabaseConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), o => o.CommandTimeout(3600)));

            services.AddIdentityCore<ListaUsuarios>()
            .AddEntityFrameworkStores<DatabaseContext>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.Configure<IdentityOptions>(opts =>
            {
                opts.Lockout.AllowedForNewUsers = true;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opts.Lockout.MaxFailedAccessAttempts = 5;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.ValueCountLimit = int.MaxValue;
            });

            services.AddSession(options =>
            {
                // options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.IdleTimeout = TimeSpan.FromHours(12);
                options.Cookie.HttpOnly = true;
            });

            //services.AddDefaultIdentity<DotacionWEBCoreUser>()
            //    .AddEntityFrameworkStores<DotacionWEBCoreContext>();

            //  services.AddIdentity<Usuario, IdentityRole>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add services required for flash message to work.
            services.AddFlashMessage();

            services.AddTransient<IVerificaRutHelpersV1, VerificaRutHelpersV1>();
            services.AddTransient<IVerificaRutHelpersV2, VerificaRutHelpersV2>();
            services.AddTransient<IPerfilUsuario, PerfilUsuario>();
            services.AddScoped<StringCase>();
            
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            // Set X-FRAME-OPTIONS
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
