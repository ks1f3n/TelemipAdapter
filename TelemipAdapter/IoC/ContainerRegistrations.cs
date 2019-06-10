using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace TelemipAdapter.IoC
{
    public static class ContainerRegistrations
    {
        public static IServiceCollection AddMQTTServiceClient(this IServiceCollection services, string mqttUrl, string mqttPort)
        {
            var factory = new MqttFactory();

            services.AddSingleton<IMqttClientOptions>(new MqttClientOptions
            {
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = mqttUrl,
                    Port = int.Parse(mqttPort)
                }
            });

            services.AddSingleton<IMqttClient>(factory.CreateMqttClient());

            return services;
        }
    }
}
