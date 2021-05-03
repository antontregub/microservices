using Facade;
using Logging;
using Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shed.CoreKit.WebApi;
using System.Net.Http;

namespace WebUI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<HttpClient>();
            services.AddWebApiEndpoints(
                new WebApiEndpoint<FacadeImpl>(new System.Uri("http://localhost:5001")),
                new WebApiEndpoint<LoggingImpl>(new System.Uri("http://localhost:5004")),
                new WebApiEndpoint<LoggingImpl>(new System.Uri("http://localhost:5005")),
                new WebApiEndpoint<LoggingImpl>(new System.Uri("http://localhost:5006")),
                new WebApiEndpoint<MessagesImpl>(new System.Uri("http://localhost:5003")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                //  when root calls, the start page will be returned
                if (string.IsNullOrEmpty(context.Request.Path.Value.Trim('/')))
                {
                    context.Request.Path = "/index.html";
                }

                await next();
            });
            app.UseStaticFiles();
            // редиректы на микросервисы
            //app.UseWebApiRedirect("api/facade", new WebApiEndpoint<FacadeImpl>(new System.Uri("http://localhost:5001")));
            //app.UseWebApiRedirect("api/logging", new WebApiEndpoint<LoggingImpl>(new System.Uri("http://localhost:5004")));
            //app.UseWebApiRedirect("api/logging", new WebApiEndpoint<LoggingImpl>(new System.Uri("http://localhost:5005")));
            //app.UseWebApiRedirect("api/logging", new WebApiEndpoint<LoggingImpl>(new System.Uri("http://localhost:5006")));
            //app.UseWebApiRedirect("api/messages", new WebApiEndpoint<MessagesImpl>(new System.Uri("http://localhost:5003")));
        }
    }
}
