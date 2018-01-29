#tool "nuget:?package=OpenCover"

var target = Argument("Target", "Test");

FilePath SolutionFile = new FilePath("HttpExtensions.sln").MakeAbsolute(Context.Environment);
FilePath ProjectFile = "src/Kralizek.Extensions.Http/Kralizek.Extensions.Http.csproj";
FilePath TestProjectFile = "tests/Tests.Extensions.Http/Tests.Extensions.Http.csproj";

var outputFolder = SolutionFile.GetDirectory().Combine("outputs");

Setup(context => 
{
    context.CleanDirectory(outputFolder);
});

Task("Build")
    .Does(() =>
{
    DotNetCoreBuild(SolutionFile.FullPath);
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    DotNetCoreTest(TestProjectFile.FullPath);
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings 
    {
        Configuration = "Release",
        OutputDirectory = outputFolder
    };
    DotNetCorePack(ProjectFile.FullPath, settings);
});

RunTarget(target);