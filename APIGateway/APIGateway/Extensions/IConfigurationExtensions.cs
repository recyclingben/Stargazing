using Microsoft.Extensions.Configuration;

namespace APIGateway
{
    public static class IConfigurationExtensions
    {
        public static string ServiceHost(this IConfiguration config, string deploymentName)
        {
            return config[$"{deploymentName.ToUpper()}_SERVICE_HOST"];
        }
    }
}