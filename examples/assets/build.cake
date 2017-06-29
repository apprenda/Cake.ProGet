#addin "nuget:?package=Cake.ProGet"

// Build Params
var Target = Argument("Target", "Default");
var Version = "0.1.0";

Task("Clean")
    .Description("Cleans any residual files")
    .Does(() => {
        DeleteFiles("./*.zip");
    });

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
        // TODO: this would be filled in as necessary
        var assetUri = string.Format("http://your-proget-server/endpoints/asset-dir/content/test-artifact-{0}.zip", Version);
        PushAsset("./artifact.zip", assetUri, new ProGetConfiguration());
    });

Task("Default")
    .Description("Performs the default task, which is 'Publish-Package'")
    .IsDependentOn("Publish-Package");

RunTarget(Target);