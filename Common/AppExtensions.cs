using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Common
{
    public class ConsulOption
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string ServiceIP { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// 服务健康检查地址
        /// </summary>
        public string ServiceHealthCheck { get; set; }

        /// <summary>
        /// Consul 地址
        /// </summary>
        public string Address { get; set; }
    }

    public static class AppExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var address = "https://localhost:8500";

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
               
                consulConfig.Address = new Uri(address);
            }));
            return services;
        }

        //public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        //{

        //    var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        //       var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
        //    var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

        //    if (!(app.Properties["server.Features"] is FeatureCollection features)) return app;

        //    //   var addresses = features.Get<IServerAddressesFeature>();
        //    // var address = addresses.Addresses.First();

        //    var address = "https://localhost:8500";

        //    Console.WriteLine($"address={address}");

        //    var uri = new Uri(address);
        //    var registration = new AgentServiceRegistration()
        //    {
        //        ID = $"MyService-{uri.Port}",
        //        Name = "ServiceAPI1",
        //        Address = $"{uri.Host}",
        //        Port = uri.Port
        //    };

        //    logger.LogInformation("Registering with Consul");
        //    consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        //    consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

        //    lifetime.ApplicationStopping.Register(() =>
        //    {
        //         logger.LogInformation("Unregistering from Consul");
        //        consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        //    });

        //    return app;
        //}

        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, ConsulOption consulOption)
        {
            var consulClient = new ConsulClient(x =>
            {
                x.Address = new Uri(consulOption.Address);
            });

            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = consulOption.ServiceName,
                Address = consulOption.ServiceIP, 
                Port = consulOption.ServicePort,
                //Check = new AgentServiceCheck()
                //{
                //    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                //    Interval = TimeSpan.FromSeconds(10),
                //    HTTP = consulOption.ServiceHealthCheck,
                //    Timeout = TimeSpan.FromSeconds(5)
                //}
            };

            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }

    }
}
