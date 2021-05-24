using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
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
            services.AddRazorPages();
            services.AddSignalR(opt =>
            {
                opt.ClientTimeoutInterval = TimeSpan.MaxValue;
                opt.KeepAliveInterval = TimeSpan.MaxValue;
                opt.EnableDetailedErrors = true;
                opt.MaximumParallelInvocationsPerClient = int.MaxValue;
                opt.MaximumReceiveMessageSize = long.MaxValue;
                opt.StreamBufferCapacity = int.MaxValue;
            });
            services.AddCors(opt => opt.AddDefaultPolicy(builder =>
            {
                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }));

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<MyHub>("/myhub");
            });
            app.Map($@"/test", innerAspNetCoreApp =>
            {
                innerAspNetCoreApp.Run(async cntx =>
                {
                    await cntx.Response.WriteAsync("Hello world");
                });
            });
        }
    }
}
