using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Cake.ProGet.Asset
{
    /// <summary>
    /// Extension methods for <see cref="ProGetConfiguration"/>
    /// </summary>
    internal static class ProGetConfigurationExtensions
    {
        internal static void Apply(this ProGetConfiguration config, HttpClient client)
        {
            if (!string.IsNullOrWhiteSpace(config.ProGetUser) && !string.IsNullOrWhiteSpace(config.ProGetPassword))
            {
                var authByteArray = Encoding.ASCII.GetBytes($"{config.ProGetUser}:{config.ProGetPassword}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authByteArray));
            }

            if (config.RequestTimeout.HasValue)
            {
                client.Timeout = config.RequestTimeout.Value;
            }
        }

        internal static void Apply(this ProGetConfiguration config, HttpWebRequest request)
        {
            if (!string.IsNullOrWhiteSpace(config.ProGetUser) && !string.IsNullOrWhiteSpace(config.ProGetPassword))
            {
                var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(config.ProGetUser + ":" + config.ProGetPassword));
                request.Headers.Add("Authorization", "Basic " + encoded);
            }

            if (config.RequestTimeout.HasValue)
            {
                request.Timeout = (int)config.RequestTimeout.Value.TotalMilliseconds;
            }
        }

        internal static HttpClient CreateClient(this ProGetConfiguration config)
        {
            HttpClient client;
            if (config.UseDefaultCredentials)
            {
                var messageHandler = new HttpClientHandler();
                messageHandler.UseDefaultCredentials = true;
                client = new HttpClient(messageHandler);
                return client;
            }
            client = new HttpClient();
            return client;
        }
    }
}