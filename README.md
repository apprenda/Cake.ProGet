# Cake.ProGet

`Cake.ProGet` is a [Cake](http://cakebuild.net) [add-in](http://cakebuild.net/docs/fundamentals/preprocessor-directives) that exposes [Inedo ProGet](https://inedo.com/proget) functionality to your build scripts.

 * Provides aliases for the functionality found in `upack.exe` ([available here](http://cdn.inedo.com/downloads/proget/upack1.0.0.zip)) .
 * Provides aliases for the [Asset Directory](https://inedo.com/support/documentation/proget/core-concepts/asset-directories) functionality.

 ## Table of Contents

1. [Building](https://github.com/apprenda/cake.proget#building)
2. [Pre-Requisites](https://github.com/apprenda/cake.proget#pre-requisites)
3. [Examples](https://github.com/apprenda/cake.proget#examples)    
    - [UPack](https://github.com/apprenda/cake.proget#universal-package)
    - [ProGet Assets](https://github.com/apprenda/cake.proget#proget-assets)
4. [Contributing](https://github.com/apprenda/cake.proget#contributing)
5. [License](https://github.com/apprenda/cake.proget#license)

## Building
This package is built using [Cake.Recipe](https://github.com/cake-contrib/Cake.Recipe)
```
> .\build.ps1
```

## Pre-Requisites

The Cake.ProGet add-in is intended to be used in conjunction with your organization's ProGet asset repository.  The alias methods for the Universal Package functionality will require your build environment to have `upack.exe` available to it.
 
 There are several ways you can configure your build environment for the `upack.exe`:
  - Add the installation path to your `%PATH%` variable.
  - Create environment variable(s) `%UPackInstall%` with the correct installation path set.
  - Host a local (to your organization) nuget package with the UPack executable and resolve the tool using a `#tool` directive.  i.e., `#tool "nuget:?package=YourPackage"`

  The order of resolution will be (first to last): local tool resolution via `#tool`, then the `%UPackInstall%` variable, and finally `%PATH%` variable.  This gives you flexibility in resolving a more specific version in a given build or allowing a system-wide default.

## Examples

### Universal Package
A full example [can be found here](https://github.com/apprenda/cake.proget/blob/develop/examples/upack)

```
#addin "nuget:?package=Cake.ProGet"

Task("Create-Package")
    .Description("Creates a universal package")    
    .Does(() => {
        Pack(new UniversalPackagePackSettings(
            "./UPack.Example.uspec",
            "./content"
        ));
    });

Task("Publish-Package")
    .Description("Publishes a universal package")
    .IsDependentOn("Create-Package")
    .Does(() => {                
        foreach(var item in GetFiles("./*.upack"))
        {
            Push(new UniversalPackagePushSettings(item.FullPath, "http://your-proget-server/upack/packages/"));
        }        
    });
```

### ProGet Assets
A full example [can be found here](https://github.com/apprenda/cake.proget/blob/develop/examples/assets)

```
#addin "nuget:?package=Cake.ProGet"

var Version = "0.1.0";

Task("Create-Package")
    .Description("Creates a package as a build artifact")
    .IsDependentOn("Clean")
    .Does(() => {
        Zip("./content", "artifact.zip");
    });

Task("Publish-Package")
    .Description("Publishes a ProGet asset")
    .IsDependentOn("Create-Package")
    .Does(() => {
        // TODO: this endpoint would be formatted based on your specific hosting environment/naming scheme.
        var assetUri = string.Format("http://your-proget-server/endpoints/asset-dir/content/test-artifact-{0}.zip", Version);
        PushAsset("./artifact.zip", assetUri, new ProGetConfiguration());
    });
```

## Contributing

If you're thinking about contributing to Cake.ProGet, please make sure you've read the [contribution guidelines](https://github.com/apprenda/cake.proget/blob/develop/CONTRIBUTING.md) before creating your first pull request.

* Fork the repository.
* Create a branch to work in.
* Make your feature addition or bug fix.
* Don't forget the unit tests.
* Send a pull request.

## License

Copyright © Apprenda Inc., and contributors.

Cake.ProGet is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/apprenda/cake.proget/blob/develop/LICENSE).

## Code of Conduct

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community.