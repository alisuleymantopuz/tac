using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Net;
using TAC.Business;
using TAC.Domain.Infrastructure;
using TAC.Shared.Helpers;

namespace TAC.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("TAC"), ServiceLifetime.Transient);
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, VehicleAvailabilityCheckerService>();
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, VehicleStatusUpdateMachineryService>();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "App/build";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TAC [Truck Availability Checker]", Version = "v1" });
            });

            var builder = new ContainerBuilder();
            builder.RegisterModule(new InfrastructureModule());
            builder.RegisterModule(new BusinessModule());
            builder.Populate(services);
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region EFWithSeeding
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
            }
            #endregion

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TAC [Truck Availability Checker] V1");
            });
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });


            #region Spa
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "App";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
            #endregion

            #region ExteptionHandler
            app.UseExceptionHandler(
                   builder =>
                   {
                       builder.Run(
                           async context =>
                           {
                               context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                               context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                               var error = context.Features.Get<IExceptionHandlerFeature>();
                               if (error != null)
                               {
                                   context.Response.Headers.Add("Application-Error", Strings.RemoveAllNonPrintableCharacters(error.Error.Message));
                                   context.Response.Headers.Add("access-control-expose-headers", "Application-Error");
                                   await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                               }
                           });
                   });
            #endregion
        }
    }
}
