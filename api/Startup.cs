using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Database;
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
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.Configure<ApiDataBaseSetttings>(
                Configuration.GetSection("LearnathonTask")
                );
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOnlineService,OnlineService>();


            services.AddTransient<IRabbitMQPublish, RabbitMQPublish>();
            services.AddTransient<IMessageRepository, MessageRepository>();


            services.AddSingleton<IDBConsumer, RabbitMQDBConsumer>(); // Need a single instance so we can keep the referenced connect with RabbitMQ open
            services.AddSingleton<ISignalRConsumer, RabitMQSignalRConsumer>();
            services.AddSignalR();
            

            // services.AddJWTTokenServices(services, builder.Configuration);
            services.AddScoped<ITokenService, TokenService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                            options => {
                                options.SaveToken = true;
                                options.RequireHttpsMetadata = false;
                                options.TokenValidationParameters = new TokenValidationParameters()
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ClockSkew = TimeSpan.Zero,

                                    ValidAudience = Configuration["JWT:ValidAudience"],
                                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                                };

                                options.Events = new JwtBearerEvents{
                                    OnMessageReceived = context => {
                                        var access_token = context.Request.Query["access_token"];
                                        var path = context.HttpContext.Request.Path;

                                        if(!string.IsNullOrEmpty(access_token) && path.StartsWithSegments("/hubs")){
                                            context.Token = access_token;
                                        }
                                        
                                        return Task.CompletedTask;
                                    }
                                };
                            }
                        
                        );

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddHttpContextAccessor();
            var multiplexer =  ConnectionMultiplexer.Connect("127.0.0.1:6379");
            IServer server = multiplexer.GetServer("127.0.0.1",6379);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddSingleton<IServer>(server);
            services.AddSingleton<IUriService>( o => {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
                            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                            {
                                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                                            Enter 'Bearer' [space] and then your token in the text input below.
                                            \r\n\r\nExample: 'Bearer 12345abcdef'",
                                Name = "Authorization",
                                In = ParameterLocation.Header,
                                Type = SecuritySchemeType.ApiKey,
                                Scheme = "Bearer"
                            });
                            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                            {
                                {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,

                                    },
                                    new List<string>()
                                }
                                });

            });


            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                            builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowAnyOrigin();
                        }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
        IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            // Configure the HTTP request pipeline.
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