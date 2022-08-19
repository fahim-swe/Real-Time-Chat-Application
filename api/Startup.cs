using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Database;
using api.Extensions;
using api.Helper;
using api.RabbitMQ;
using api.Services;
using api.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDatabaseServices(_config);
            services.AddApplicationServices(_config);
            services.AddRabbitMQServices(_config);
            
            services.AddControllers();
            
            services.AddSwaggerService(_config);
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                            builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowAnyOrigin();
                        }));
            
            services.AddIdentityService(_config);
        
        }

    
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
           
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints( endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");
                endpoints.MapHub<MessageHub>("hubs/message");
            });


            lifetime.ApplicationStarted.Register(() => RegisterSignalRWithRabbitMQ(app.ApplicationServices));

        }



        public void RegisterSignalRWithRabbitMQ(IServiceProvider serviceProvider)
        {
            // Connect to RabbitMQ
            var rabbitMQService1 = (ISignalRConsumer?)serviceProvider.GetService(typeof(ISignalRConsumer));
            if(rabbitMQService1 != null){
                rabbitMQService1.Connect();
            }


            var rabbitMQService2 = (IDBConsumer?)serviceProvider.GetService(typeof(IDBConsumer));
            if(rabbitMQService2 != null){
                rabbitMQService2.Connect();
            }
        }
    }
}