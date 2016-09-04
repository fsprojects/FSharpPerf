// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"packages/build/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Git
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper
open Fake.UserInputHelper
open System
open System.IO


let runs = 10
let compilerForkToTest = @"https://github.com/forki/visualfsharp"
let compilerHashes = ["71c8798e19d6e15d3e6a98c80da658aa5ed2c630"; "1bf329fa06b7e2e4d4ceab545b0e059e72be3e1c"]

// --------------------------------------------------------------------------------------
// START TODO: Provide project-specific details below
// --------------------------------------------------------------------------------------

// Information about the project are used
//  - for version and project name in generated AssemblyInfo file
//  - by the generated NuGet package
//  - to run tests and to publish documentation on GitHub gh-pages
//  - for documentation, you also need to edit info in "docs/tools/generate.fsx"

// The name of the project
// (used by attributes in AssemblyInfo, name of a NuGet package and directory in 'src')
let project = "FSharpPerf"

// Short summary of the project
// (used as description in AssemblyInfo and as a short summary for NuGet package)
let summary = "A set of performance test scripts for the F# compiler."

// Longer description of the project
// (used as a description for NuGet package; line breaks are automatically cleaned up)
let description = "Project has no description; update build.fsx"

// List of author names (for NuGet package)
let authors = [ "Steffen Forkmann" ]

// Tags for your project (for NuGet package)
let tags = "F# fsharp performance"

// File system information
let solutionFile  = "FSharpPerf.sln"

// Pattern specifying assemblies to be tested using NUnit
let testAssemblies = "tests/**/bin/Release/*Tests*.dll"

// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted
let gitOwner = "fsprojects"
let gitHome = sprintf "%s/%s" "https://github.com" gitOwner

// The name of the project on GitHub
let gitName = "FSharpPerf"

// The url for the raw files hosted
let gitRaw = environVarOrDefault "gitRaw" "https://raw.githubusercontent.com/fsprojects"

// --------------------------------------------------------------------------------------
// END TODO: The rest of the file includes standard build steps
// --------------------------------------------------------------------------------------

let release = LoadReleaseNotes "RELEASE_NOTES.md"

let rootDir = "perftests"
let compilerDir = Path.GetFullPath "./compiler"

Target "Clean" (fun _ ->
    CleanDirs [ rootDir; compilerDir ]
)

Target "CleanDocs" (fun _ ->
    CleanDirs ["docs/output"]
)

let projectDir name = Path.Combine(rootDir,name) |> FullName
let projects =
    [ "Paket", "https://github.com/fsprojects/Paket.git", "build.cmd", "src/Paket.Core", """-o:obj\Release\Paket.Core.dll -g --debug:pdbonly --noframework --define:TRACE --doc:..\..\bin\Paket.Core.XML --optimize+ -r:..\..\packages\Chessie\lib\net40\Chessie.dll -r:..\..\packages\FSharp.Core\lib\net40\FSharp.Core.dll -r:..\..\packages\Mono.Cecil\lib\net45\Mono.Cecil.dll -r:..\..\packages\Mono.Cecil\lib\net45\Mono.Cecil.Mdb.dll -r:..\..\packages\Mono.Cecil\lib\net45\Mono.Cecil.Pdb.dll -r:..\..\packages\Mono.Cecil\lib\net45\Mono.Cecil.Rocks.dll -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\mscorlib.dll" -r:..\..\packages\Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Configuration.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Data.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Data.Linq.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.IO.Compression.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.IO.Compression.FileSystem.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Numerics.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Security.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.Linq.dll" --target:library --warn:3 --warnaserror:76 --vserrors --validate-type-providers --LCID:1033 --utf8output --fullpaths --flaterrors --subsystemversion:6.00 --highentropyva+ --sqmsessionguid:bcb8c335-41a6-4173-9636-2335b6448248 ..\..\paket-files\fsprojects\FSharp.TypeProviders.StarterPack\src\AssemblyReader.fs ..\..\paket-files\fsharp\FAKE\src\app\FakeLib\Globbing\Globbing.fs Async.fs AssemblyInfo.fs CustomAssemblyInfo.fs Domain.fs Constants.fs Logging.fsi Logging.fs Utils.fs SemVer.fs VersionRange.fs Xml.fs GitCommandHelper.fs GitHandling.fs PlatformDetection.fs ConfigFile.fs Cache.fs Cultures.fs PackageSources.fs FrameworkHandling.fs PlatformMatching.fs Requirements.fs ModuleResolver.fs RemoteDownload.fs RemoteUpload.fs PackageResolver.fs Nuspec.fs InstallModel.fs ReferencesFile.fs SolutionFile.fs Nuget.fs NuGetV3.fs NuGetV2.fs DependenciesTypes.fs DependenciesFileParser.fs LockFile.fs TemplateFile.fs ProjectFile.fs DependenciesFile.fs LocalFile.fs DependencyChangeDetection.fs GarbageCollection.fs RestoreProcess.fs BindingRedirects.fs NupkgWriter.fs ProcessOptions.fs PackagesConfigFile.fs DependencyModel.fs InstallProcess.fs UpdateProcess.fs RemoveProcess.fs AddProcess.fs PackageMetaData.fs PackageProcess.fs Environment.fs Releases.fs Simplifier.fs VSIntegration.fs NugetConvert.fs FindOutdated.fs FindReferences.fs PublicAPI.fs ScriptGeneration.fs"""
      "FSharpx.Collections", "https://github.com/fsprojects/FSharpx.Collections.git", "build.cmd", "src/FSharpx.Collections" , """-o:obj\Debug\FSharpx.Collections.dll -g --debug:full --noframework --optimize- --tailcalls- -r:..\..\packages\FSharp.Core\lib\net40\FSharp.Core.dll -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\mscorlib.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.dll" --target:library --warn:3 --warnaserror:76 --vserrors --validate-type-providers --LCID:1033 --utf8output --fullpaths --flaterrors --highentropyva- --sqmsessionguid:69dfdfc7-1aa9-4121-9408-d55ff440afae AssemblyInfo.fs Exceptions.fs Interfaces.fs Infrastructure.fs LazyList.fsi LazyList.fs ResizeArray.fsi ResizeArray.fs Collections.fs Deque.fsi Deque.fs DList.fsi DList.fs ByteString.fs CircularBuffer.fs PriorityQueue.fs NonEmptyList.fs RandomAccessList.fsi RandomAccessList.fs Queue.fsi Queue.fs Literals.fs PersistentVector.fsi PersistentVector.fs PersistentHashMap.fs"""
      "FSharp.Data", "https://github.com/fsharp/FSharp.Data.git", "build.cmd", "src" , """-o:obj\Debug\FSharp.Data.dll -g --debug:full --noframework --define:DEBUG --define:TRACE --optimize- --tailcalls- -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.0.0\FSharp.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\mscorlib.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Xml.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Xml.Linq.dll" --target:library --warn:3 --warnaserror:76 --vserrors --validate-type-providers --LCID:1033 --utf8output --fullpaths --flaterrors --highentropyva- --sqmsessionguid:60edec96-3e22-4317-b40f-03727d8ed775 --warnon:1182 Net\UriUtils.fs Net\Http.fs CommonRuntime\IO.fs CommonRuntime\Caching.fs CommonRuntime\TextConversions.fs CommonRuntime\TextRuntime.fs CommonRuntime\Pluralizer.fs CommonRuntime\NameUtils.fs CommonRuntime\StructuralTypes.fs CommonRuntime\StructuralInference.fs Json\JsonValue.fs Json\JsonConversions.fs Json\JsonExtensions.fs Json\JsonRuntime.fs Xml\XmlRuntime.fs Csv\CsvRuntime.fs Csv\CsvFile.fs Csv\CsvExtensions.fs Csv\CsvInference.fs WorldBank\WorldBankRuntime.fs Html\HtmlCharRefs.fs Html\HtmlParser.fs Html\HtmlOperations.fs Html\HtmlCssSelectors.fs Html\HtmlInference.fs Html\HtmlRuntime.fs Runtime.fs AssemblyInfo.fs"""]

let clone gitUrl name = Repository.clone rootDir gitUrl (projectDir name)

Target "CloneProjects" (fun _ ->
    for name,gitUrl,_,_,_ in projects do
        clone gitUrl name
)

Target "BuildProjects" (fun _ ->
    for name,_,command,_,_ in projects do
        let result = 
            ExecProcess (fun info -> 
                info.FileName <- Path.Combine(projectDir name,command) 
                info.WorkingDirectory <- projectDir name) System.TimeSpan.MaxValue
        if result <> 0 then failwithf "Error during build of %s" name
)

Target "RunPerfTests" (fun _ ->
    let allTimings = System.Collections.Generic.List<_>()
    for hash in compilerHashes do
        CleanDirs [ compilerDir ]
        Repository.clone "" compilerForkToTest compilerDir
        Git.Branches.checkoutNewBranch compilerDir hash "test"

        let result = 
            ExecProcess (fun info -> 
                info.FileName <- Path.Combine(compilerDir,"build.cmd") 
                info.WorkingDirectory <- compilerDir) System.TimeSpan.MaxValue
        if result <> 0 then failwithf "Error during build of compiler %s" hash

        let compilerExe = Path.Combine (compilerDir, @"release\net40\bin\fsc.exe")

        let times = System.Collections.Generic.Dictionary<_,_>()
        for run in 1..runs do
            for name,_,_,dir,args in projects do
                let sw = System.Diagnostics.Stopwatch.StartNew()
                let result = 
                    ExecProcess (fun info -> 
                        info.FileName <- compilerExe
                        info.Arguments <- args
                        info.WorkingDirectory <- Path.Combine(projectDir name,dir)) System.TimeSpan.MaxValue
                if result <> 0 then failwithf "Error during compile test of %s" name
                times.Add((name,run),sw.ElapsedMilliseconds)

        allTimings.Add(hash,times)


    for hash,times in allTimings do
        tracefn "Compiler: %s" hash
        let timesPerProject =
            times 
            |> Seq.groupBy (fun kv -> kv.Key |> fst)
            |> Seq.map (fun (name,times) -> name,times |> Seq.sumBy (fun kv -> kv.Value))
            |> Seq.toList

        timesPerProject
        |> Seq.iter (fun (name,runtime) -> tracefn "   Project: %s %Ams" name (float (runtime / int64 runs)))

        tracefn ""
        timesPerProject
        |> Seq.sumBy (fun (_,runtime) -> runtime)
        |> fun runtime -> tracefn "   Average Run: %Ams" (float (runtime / int64 runs))
)

// --------------------------------------------------------------------------------------
// Generate the documentation


let fakePath = "packages" </> "build" </> "FAKE" </> "tools" </> "FAKE.exe"
let fakeStartInfo script workingDirectory args fsiargs environmentVars =
    (fun (info: System.Diagnostics.ProcessStartInfo) ->
        info.FileName <- System.IO.Path.GetFullPath fakePath
        info.Arguments <- sprintf "%s --fsiargs -d:FAKE %s \"%s\"" args fsiargs script
        info.WorkingDirectory <- workingDirectory
        let setVar k v =
            info.EnvironmentVariables.[k] <- v
        for (k, v) in environmentVars do
            setVar k v
        setVar "MSBuild" msBuildExe
        setVar "GIT" Git.CommandHelper.gitPath
        setVar "FSI" fsiPath)

/// Run the given buildscript with FAKE.exe
let executeFAKEWithOutput workingDirectory script fsiargs envArgs =
    let exitCode =
        ExecProcessWithLambdas
            (fakeStartInfo script workingDirectory "" fsiargs envArgs)
            TimeSpan.MaxValue false ignore ignore
    System.Threading.Thread.Sleep 1000
    exitCode

// Documentation
let buildDocumentationTarget fsiargs target =
    trace (sprintf "Building documentation (%s), this could take some time, please wait..." target)
    let exit = executeFAKEWithOutput "docs/tools" "generate.fsx" fsiargs ["target", target]
    if exit <> 0 then
        failwith "generating reference documentation failed"
    ()

Target "GenerateReferenceDocs" (fun _ ->
    buildDocumentationTarget "-d:RELEASE -d:REFERENCE" "Default"
)

let generateHelp' fail debug =
    let args =
        if debug then "--define:HELP"
        else "--define:RELEASE --define:HELP"
    try
        buildDocumentationTarget args "Default"
        traceImportant "Help generated"
    with
    | e when not fail ->
        traceImportant "generating help documentation failed"

let generateHelp fail =
    generateHelp' fail false

Target "GenerateHelp" (fun _ ->
    DeleteFile "docs/content/release-notes.md"
    CopyFile "docs/content/" "RELEASE_NOTES.md"
    Rename "docs/content/release-notes.md" "docs/content/RELEASE_NOTES.md"

    DeleteFile "docs/content/license.md"
    CopyFile "docs/content/" "LICENSE.txt"
    Rename "docs/content/license.md" "docs/content/LICENSE.txt"

    generateHelp true
)

Target "GenerateHelpDebug" (fun _ ->
    DeleteFile "docs/content/release-notes.md"
    CopyFile "docs/content/" "RELEASE_NOTES.md"
    Rename "docs/content/release-notes.md" "docs/content/RELEASE_NOTES.md"

    DeleteFile "docs/content/license.md"
    CopyFile "docs/content/" "LICENSE.txt"
    Rename "docs/content/license.md" "docs/content/LICENSE.txt"

    generateHelp' true true
)

Target "KeepRunning" (fun _ ->
    use watcher = !! "docs/content/**/*.*" |> WatchChanges (fun changes ->
         generateHelp' true true
    )

    traceImportant "Waiting for help edits. Press any key to stop."

    System.Console.ReadKey() |> ignore

    watcher.Dispose()
)

Target "GenerateDocs" DoNothing

let createIndexFsx lang =
    let content = """(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../../bin"

(**
F# Project Scaffold ({0})
=========================
*)
"""
    let targetDir = "docs/content" </> lang
    let targetFile = targetDir </> "index.fsx"
    ensureDirectory targetDir
    System.IO.File.WriteAllText(targetFile, System.String.Format(content, lang))

Target "AddLangDocs" (fun _ ->
    let args = System.Environment.GetCommandLineArgs()
    if args.Length < 4 then
        failwith "Language not specified."

    args.[3..]
    |> Seq.iter (fun lang ->
        if lang.Length <> 2 && lang.Length <> 3 then
            failwithf "Language must be 2 or 3 characters (ex. 'de', 'fr', 'ja', 'gsw', etc.): %s" lang

        let templateFileName = "template.cshtml"
        let templateDir = "docs/tools/templates"
        let langTemplateDir = templateDir </> lang
        let langTemplateFileName = langTemplateDir </> templateFileName

        if System.IO.File.Exists(langTemplateFileName) then
            failwithf "Documents for specified language '%s' have already been added." lang

        ensureDirectory langTemplateDir
        Copy langTemplateDir [ templateDir </> templateFileName ]

        createIndexFsx lang)
)

// --------------------------------------------------------------------------------------
// Release Scripts

Target "ReleaseDocs" (fun _ ->
    let tempDocsDir = "temp/gh-pages"
    CleanDir tempDocsDir
    Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

    CopyRecursive "docs/output" tempDocsDir true |> tracefn "%A"
    StageAll tempDocsDir
    Git.Commit.Commit tempDocsDir (sprintf "Update generated documentation for version %s" release.NugetVersion)
    Branches.push tempDocsDir
)

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing
Target "CleanPerfTests" DoNothing

"Clean"
  ==> "CloneProjects"
  ==> "BuildProjects"
  ==> "CleanPerfTests"
  ==> "All"

"RunPerfTests"
  ==> "CleanPerfTests"

"CleanDocs"
  ==> "All"
  ==> "GenerateHelp"
  ==> "GenerateReferenceDocs"
  ==> "GenerateDocs"  
  ==> "ReleaseDocs"

"CleanDocs"
  ==> "GenerateHelpDebug"
  ==> "KeepRunning"
  
RunTargetOrDefault "All"
