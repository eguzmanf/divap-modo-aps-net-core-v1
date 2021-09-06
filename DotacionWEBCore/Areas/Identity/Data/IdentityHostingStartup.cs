using DotacionWEBCore.Areas.Identity.Data;
using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

[assembly: HostingStartup(typeof(DotacionWEBCore.Areas.Identity.IdentityHostingStartupppp))]
namespace DotacionWEBCore.Areas.Identity
{
    public class IdentityHostingStartupppp : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DotacionWEBCoreContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DotacionWEBCoreContextConnection")));

               services.AddDefaultIdentity<DotacionWEBCoreUser>()
               .AddEntityFrameworkStores<DotacionWEBCoreContext>();
               
               //services.AddIdentity<DotacionWEBCoreUser, IdentityRole>()
               //     .AddRoles<IdentityRole>()
               //     .AddEntityFrameworkStores<DotacionWEBCoreContext>();
                 
            });

            builder.ConfigureServices((context, services) => {
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json")
            .Build());

            services.AddMvc();
            
            });
        }

    }
}
