using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.ServiceUtility.ClusterRequests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using APIGateway.SignalR.Stars.StarUpdates;
using APIGateway.SignalR.APIGateway.StarUpdates;
using APIGateway.ServiceUtility.StarUpdatesDataManagement;

namespace APIGateway
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR();
            services.AddCors(o => o.AddPolicy("FullCors", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSingleton<IClusterRequestAgent, ClusterRequestAgent>();
            services.AddSingleton<StarUpdatesEmitter>();
            services.AddSingleton<StarUpdatesListener>();

            services.AddSingleton<StarIncrementationAgreggator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSignalR(routes =>
            {
                routes.MapHub<StarUpdatesHub>("/signalr/starupdates");
            });

            app.ApplicationServices.GetService<StarUpdatesListener>().StartAsync().GetAwaiter().GetResult();
            ActivatorUtilities.CreateInstance<StarUpdatesClientUpdater>(app.ApplicationServices);

            app.UseCors();
            app.UseMvc();
        }
    }
}
