using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSignalR.Hubs;
using Microsoft.AspNetCore.Cors.Infrastructure;
using CoreSignalR.Models;

namespace CoreSignalR
{
    public class Startup
    {
        public static List<TagItemModel> lastValues = new List<TagItemModel>();
        public static List<TagItemModel> tempValues = new List<TagItemModel>();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
              options => options.AddPolicy("AllowCors",
                  builder =>
                  {
                      builder
                          .SetIsOriginAllowed(origin => true)
                          .AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                  })
          );
            services.AddRazorPages();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("AllowCors");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                //endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<KilicHub>("/demoHub");
            });
        }
    }
}
