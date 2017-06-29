#addin "nuget:?package=Cake.ProGet"

// Build Params
var Target = Argument("Target", "Default");
var ProgetUniversalPackageEndpoint = Argument("ProgetUniversalPackageEndpoint", "http://your-proget-server/upack/packages/");

Task("Clean")
    .Description("Cleans any residual files")
    .Does(() => {
        DeleteFiles("./*.upack");
    });

Task("Create-Package")
    .Description("Creates a universal package")
    .IsDependentOn("Clean")
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
            Push(new UniversalPackagePushSettings(item.FullPath, ProgetUniversalPackageEndpoint));
        }        
    });

Task("Default")
    .Description("Performs the default task, which is 'Publish-Package'")
    .IsDependentOn("Publish-Package");

RunTarget(Target);