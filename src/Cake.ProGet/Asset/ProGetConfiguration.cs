using System;
//// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Cake.ProGet.Asset
{
    /// <summary>
    /// The username and password for the ProGet instance.
    /// </summary>
    public sealed class ProGetConfiguration 
    {
        /// <summary>
        /// Gets or sets the ProGet Username
        /// </summary>
        public string ProGetUser { get; set; }

        /// <summary>
        /// Gets or sets the ProGet Password
        /// </summary>
        public string ProGetPassword { get; set; }

        /// <summary>
        /// Gets or sets the request timeout.
        /// </summary>
        /// <value>
        /// The request timeout.
        /// </value>
        public TimeSpan? RequestTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether defaultcredentials (executing user) used for authenticating against ProGet
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
    }
}