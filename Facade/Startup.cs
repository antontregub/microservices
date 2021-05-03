using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shed.CoreKit.WebApi;


namespace Facade
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCorrelationToken();
            services.AddCors();
            // регистрируем реализацию как зависимость в контейнере IoC
            services.AddTransient<FacadeImpl, FacadeImpl>();
            services.AddLogging(builder => builder.AddConsole());
           // services.AddRequestLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

            app.UseCorrelationToken();
            //app.UseRequestLogging();
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            // привязываем реализацию к конечной точке
            app.UseWebApiEndpoint<FacadeImpl>();
        }
    }
}
