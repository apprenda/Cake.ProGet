using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Cake.Core;
using Newtonsoft.Json;

namespace Cake.ProGet.Asset
{
    /// <summary>
    /// Provides methods to list or create asset directories in ProGet.
    /// </summary>
    internal sealed class ProGetAssetDirectoryLister
    {
        private readonly ProGetConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProGetAssetDirectoryLister"/> class.
        /// </summary>
        /// <param name="configuration">The ProGet Configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown if environment, log, or config are null.</exception>
        public ProGetAssetDirectoryLister(ProGetConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        /// <summary>
        /// Creates a new Asset Directory at the specified path.
        /// </summary>
        /// <param name="directoryUri">The fullly qualified URI to the new directory.</param>
        /// <exception cref="CakeException">Thrown if authorization fails.</exception>
        public void CreateDirectory(string directoryUri)
        {
            var client = _configuration.CreateClient();
            _configuration.Apply(client);
            
            var result = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, directoryUri)).Result;

            if (result.StatusCode.Equals(HttpStatusCode.Unauthorized) || result.StatusCode.Equals(HttpStatusCode.Forbidden))
            {
                throw new CakeException("Authorization to ProGet server failed; Credentials were incorrect, or not supplied.");
            }
        }

        /// <summary>
        /// Lists a ProGet Asset Directory.
        /// </summary>
        /// <param name="directoryUri">The URI of the directory to list.</param>
        /// <param name="recursive">If true, recurse through subdirectories as well. Defaults to false.</param>
        /// <returns>A list of <see cref="ProGetDirectoryListing"/> items.</returns>
        /// <exception cref="CakeException">Throws if authorization fails.</exception>
        public List<ProGetDirectoryListing> ListDirectory(string directoryUri, bool recursive = false)
        {
            var client = _configuration.CreateClient();
            _configuration.Apply(client);

            var result = client
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, $"{directoryUri}?recursive={recursive.ToString().ToLower()}")).Result;
            
            if (result.StatusCode.Equals(HttpStatusCode.Unauthorized) || result.StatusCode.Equals(HttpStatusCode.Forbidden))
            {
                throw new CakeException("Authorization to ProGet server failed; Credentials were incorrect, or not supplied.");
            }

            return JsonConvert.DeserializeObject<List<ProGetDirectoryListing>>(
                result.Content.ReadAsStringAsync().Result);
        }
    }
}