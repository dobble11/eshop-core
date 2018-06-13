using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using EShop.Data;

namespace EShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options=> options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");

            services.AddDbContext<EShopContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EShopContext")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,EShopContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();
            app.UseMvc();

            //DbInit.Init(context);
        }
    }
}
