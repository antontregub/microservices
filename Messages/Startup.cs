using System.Net.Http;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Middleware;
using Shed.CoreKit.WebApi;

namespace Messages
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationToken();
            services.AddCors();
            services.AddTransient<IMessages, MessagesImpl>();
            services.AddTransient<HttpClient>();
            services.AddWebApiEndpoints(new WebApiEndpoint<IFacade>(new System.Uri("http://localhost:5001")));
            // регистрируем планировщик (запустится при старте приложения, больше ничего делать не нужно)
            //services.AddHostedService<Scheduler>();
            services.AddLogging(builder => builder.AddConsole());
            services.AddRequestLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelationToken();
            //app.UseRequestLogging("get");
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseWebApiEndpoint<IMessages>();
        }
    }
}
