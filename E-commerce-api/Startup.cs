using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopBridge.AutoMapper;
using ShopBridge.Data.DataContext;
using ShopBridge.Data.Repository;
using ShopBridge.Interfaces;
using ShopBridge.Interfaces.Repository;
using ShopBridge.Services;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace ShopBridge
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
            services.AddControllers()
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ContractResolver = new DefaultContractResolver();
               });
            string connectionString = Configuration.GetValue<string>("DbConnectionString");

            // ===== Add our DbContext ========
            services.AddDbContext<InventoryDbContext>(x => x.UseMySql(connectionString));
            var autoMapper = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfiler())).CreateMapper();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddSingleton(autoMapper);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
