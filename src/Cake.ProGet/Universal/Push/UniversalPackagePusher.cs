﻿using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal.Push
{
    /// <summary>
    /// Packs a universal package archive
    /// </summary>
    /// <seealso cref="UPackTool{UniversalPackagePackSettings}" />
    public sealed class UniversalPackagePusher : UPackTool<UniversalPackagePushSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackagePusher"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tools.</param>
        /// <param name="resolver">The resolver.</param>
        public UniversalPackagePusher(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, IUPackToolResolver resolver)
            : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Executes the command using the specified settings
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Execute(UniversalPackagePushSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.Package == null)
            {
                throw new CakeException("Required setting Package not specified.");
            }

            if (string.IsNullOrEmpty(settings.Target))
            {
                throw new CakeException("Required setting Target not specified.");
            }

            if (!this.FileSystem.GetFile(settings.Package).Exists)
            {
                throw new CakeException($"Universal package file does not exist at '{settings.Package.FullPath}'");
            }

            var builder = new ProcessArgumentBuilder();

            builder.Append("push");

            builder.AppendQuoted(settings.Package.MakeAbsolute(Environment).FullPath);
            builder.AppendQuoted(settings.Target);

            if (settings.HasCredentials())
            {
                if (!settings.AreCredentialsValid())
                {
                    throw new CakeException("Both username and password must be specified for authentication");
                }
                
                // this is a bit hacky.  ProGet Universal Package endpoints expect a particular format
                // in which we want to protect the secret, so we stitch together the switch syntax
                builder.AppendSwitchSecret($"--user={settings.UserName}", ":", settings.Password);
            }

            Run(settings, builder);
        }
    }
}
