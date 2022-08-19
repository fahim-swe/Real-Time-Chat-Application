using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Database;
using api.Helper;
using api.RabbitMQ;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<ApiDataBaseSetttings>(
                 _config.GetSection("LearnathonTask")
                );

            return services;
        }
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration _config)
        {

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOnlineService,OnlineService>();
            services.AddTransient<IMessageRepository, MessageRepository>();

          
            services.AddSignalR();


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
            

            return services;
        }
    
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration _config)
        {
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

                                    ValidAudience = _config["JWT:ValidAudience"],
                                    ValidIssuer = _config["JWT:ValidIssuer"],
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]))
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
            return services;
        }
    
        public static IServiceCollection AddRabbitMQServices(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<RabbitMQConnectionFactorySettings>(
                 _config.GetSection("RabbitMQConectionFactory"));

        
            services.AddScoped<IRabbitMQPublish, RabbitMQPublish>();
            services.AddSingleton<IDBConsumer, RabbitMQDBConsumer>();
            services.AddSingleton<ISignalRConsumer, RabitMQSignalRConsumer>();
            return services;
        }

        public static IServiceCollection AddSwaggerService(this IServiceCollection services, IConfiguration _config)
        {
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
            return services;
        }
    }
}