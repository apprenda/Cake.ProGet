using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.ProGet.Asset
{
    /// <summary>
    /// Downloads assets from ProGet Asset Directories.
    /// </summary>
    internal sealed class ProGetAssetDownloader
    {
        private readonly ProGetConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProGetAssetDownloader"/> class.
        /// </summary>
        /// <param name="configuration">The ProGet Configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown if environment, log, or config are null.</exception>
        public ProGetAssetDownloader(ProGetConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        public void GetSingleAsset(string assetUri, FilePath outputPath)
        {
            var client = _configuration.CreateClient();
            _configuration.Apply(client);

            var response = client.GetAsync(assetUri).Result;
            
            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized) ||
                response.StatusCode.Equals(HttpStatusCode.Forbidden))
            {
                throw new CakeException("Authorization to ProGet server failed; Credentials were incorrect, or not supplied.");
            }

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                throw new CakeException("The asset was not found.");
            }
                                    
            using (var streamToReadFrom = response.Content.ReadAsStreamAsync().Result)
            using (var file = new FileStream(outputPath.FullPath, FileMode.CreateNew, FileAccess.Write))
            {
                streamToReadFrom.CopyTo(file);
            }
        }

        public void GetDirectoryOfAssets(string assetDirectoryUri, FilePath outputPath)
        {
            var client = _configuration.CreateClient();
            _configuration.Apply(client);
            
            var response = client.GetAsync($"{assetDirectoryUri}?format=zip&recursive=true").Result;
            
            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized) ||
                response.StatusCode.Equals(HttpStatusCode.Forbidden))
            {
                throw new CakeException("Authorization to ProGet server failed; Credentials were incorrect, or not supplied.");
            }
            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                throw new CakeException("The asset was not found.");
            }
            
            using (var streamToReadFrom = response.Content.ReadAsStreamAsync().Result)
            using (var file = new FileStream(outputPath.FullPath, FileMode.CreateNew, FileAccess.Write))
            {
                streamToReadFrom.CopyTo(file);
            }
        }
    }
}