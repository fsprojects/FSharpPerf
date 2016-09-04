// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"packages/build/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Git
open System
open System.IO


let runs = 10
let compilerForkToTest = @"https://github.com/forki/visualfsharp"
let compilerHashes = ["71c8798e19d6e15d3e6a98c80da658aa5ed2c630"; "1bf329fa06b7e2e4d4ceab545b0e059e72be3e1c"]

let additionalFlags = "--typecheckonly"

let projects =
    [ "Paket", "https://github.com/fsprojects/Paket.git", "build.cmd", "src/Paket.Core", """-o:obj\Release\Paket.Core.dll -g --debug:pdbonly --noframework --define:TRACE --doc:..\..\bin\Paket.Core.XML --optimize+ -r:..\..\packages\Chessie\lib\net40\Chessie.dll -r:..\..\packages\FSharp.Core\lib\net40\FSharp.Core.dll -r:..\..\packages\Mono.Cecil\lib\net45\Mono.Cecil.dll -r:..\..\packages\Mono.Cecil\lib\net45\Mono.Cecil.Mdb.dll -r:..\..\packages\Mono.Cecil\lib\net45\Mono.Cecil.Pdb.dll -r:..\..\packages\Mono.Cecil\lib\net45\Mono.Cecil.Rocks.dll -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\mscorlib.dll" -r:..\..\packages\Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Configuration.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Data.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Data.Linq.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.IO.Compression.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.IO.Compression.FileSystem.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Numerics.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Security.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.Linq.dll" --target:library --warn:3 --warnaserror:76 --vserrors --validate-type-providers --LCID:1033 --utf8output --fullpaths --flaterrors --subsystemversion:6.00 --highentropyva+ --sqmsessionguid:bcb8c335-41a6-4173-9636-2335b6448248 ..\..\paket-files\fsprojects\FSharp.TypeProviders.StarterPack\src\AssemblyReader.fs ..\..\paket-files\fsharp\FAKE\src\app\FakeLib\Globbing\Globbing.fs Async.fs AssemblyInfo.fs CustomAssemblyInfo.fs Domain.fs Constants.fs Logging.fsi Logging.fs Utils.fs SemVer.fs VersionRange.fs Xml.fs GitCommandHelper.fs GitHandling.fs PlatformDetection.fs ConfigFile.fs Cache.fs Cultures.fs PackageSources.fs FrameworkHandling.fs PlatformMatching.fs Requirements.fs ModuleResolver.fs RemoteDownload.fs RemoteUpload.fs PackageResolver.fs Nuspec.fs InstallModel.fs ReferencesFile.fs SolutionFile.fs Nuget.fs NuGetV3.fs NuGetV2.fs DependenciesTypes.fs DependenciesFileParser.fs LockFile.fs TemplateFile.fs ProjectFile.fs DependenciesFile.fs LocalFile.fs DependencyChangeDetection.fs GarbageCollection.fs RestoreProcess.fs BindingRedirects.fs NupkgWriter.fs ProcessOptions.fs PackagesConfigFile.fs DependencyModel.fs InstallProcess.fs UpdateProcess.fs RemoveProcess.fs AddProcess.fs PackageMetaData.fs PackageProcess.fs Environment.fs Releases.fs Simplifier.fs VSIntegration.fs NugetConvert.fs FindOutdated.fs FindReferences.fs PublicAPI.fs ScriptGeneration.fs"""
      "FSharpx.Collections", "https://github.com/fsprojects/FSharpx.Collections.git", "build.cmd", "src/FSharpx.Collections" , """-o:obj\Debug\FSharpx.Collections.dll -g --debug:full --noframework --optimize- --tailcalls- -r:..\..\packages\FSharp.Core\lib\net40\FSharp.Core.dll -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\mscorlib.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.dll" --target:library --warn:3 --warnaserror:76 --vserrors --validate-type-providers --LCID:1033 --utf8output --fullpaths --flaterrors --highentropyva- --sqmsessionguid:69dfdfc7-1aa9-4121-9408-d55ff440afae AssemblyInfo.fs Exceptions.fs Interfaces.fs Infrastructure.fs LazyList.fsi LazyList.fs ResizeArray.fsi ResizeArray.fs Collections.fs Deque.fsi Deque.fs DList.fsi DList.fs ByteString.fs CircularBuffer.fs PriorityQueue.fs NonEmptyList.fs RandomAccessList.fsi RandomAccessList.fs Queue.fsi Queue.fs Literals.fs PersistentVector.fsi PersistentVector.fs PersistentHashMap.fs"""
      "FSharp.Data", "https://github.com/fsharp/FSharp.Data.git", "build.cmd", "src" , """-o:obj\Debug\FSharp.Data.dll -g --debug:full --noframework --define:DEBUG --define:TRACE --optimize- --tailcalls- -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.0.0\FSharp.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\mscorlib.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Xml.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Xml.Linq.dll" --target:library --warn:3 --warnaserror:76 --vserrors --validate-type-providers --LCID:1033 --utf8output --fullpaths --flaterrors --highentropyva- --sqmsessionguid:60edec96-3e22-4317-b40f-03727d8ed775 --warnon:1182 Net\UriUtils.fs Net\Http.fs CommonRuntime\IO.fs CommonRuntime\Caching.fs CommonRuntime\TextConversions.fs CommonRuntime\TextRuntime.fs CommonRuntime\Pluralizer.fs CommonRuntime\NameUtils.fs CommonRuntime\StructuralTypes.fs CommonRuntime\StructuralInference.fs Json\JsonValue.fs Json\JsonConversions.fs Json\JsonExtensions.fs Json\JsonRuntime.fs Xml\XmlRuntime.fs Csv\CsvRuntime.fs Csv\CsvFile.fs Csv\CsvExtensions.fs Csv\CsvInference.fs WorldBank\WorldBankRuntime.fs Html\HtmlCharRefs.fs Html\HtmlParser.fs Html\HtmlOperations.fs Html\HtmlCssSelectors.fs Html\HtmlInference.fs Html\HtmlRuntime.fs Runtime.fs AssemblyInfo.fs"""
      "SQLProvider", "https://github.com/fsprojects/SQLProvider.git", "build.cmd", "src/SQLProvider", """-o:obj\Release\FSharp.Data.SqlProvider.dll --debug:pdbonly --noframework --define:TRACE --doc:..\..\bin\FSharp.Data.SqlProvider.XML --optimize+ -r:..\..\packages\FSharp.Core\lib\net40\FSharp.Core.dll -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\mscorlib.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Configuration.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Core.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Data.DataSetExtensions.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Data.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.IdentityModel.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Runtime.Serialization.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.ServiceModel.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Transactions.dll" -r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Xml.dll" --target:library --warn:3 --warnaserror:76 --fullpaths --flaterrors --subsystemversion:6.00 --highentropyva+ AssemblyInfo.fs ..\..\paket-files\fsprojects\FSharp.TypeProviders.StarterPack\src\ProvidedTypes.fsi ..\..\paket-files\fsprojects\FSharp.TypeProviders.StarterPack\src\ProvidedTypes.fs ..\..\paket-files\Thorium\Linq.Expression.Optimizer\src\Linq.Expression.Optimizer\ExpressionOptimizer.fs Operators.fs Utils.fs SqlSchema.fs DataTable.fs SqlRuntime.Patterns.fs QuotationHelpers.fs SqlRuntime.Common.fs Providers.MsSqlServer.fs Providers.MSAccess.fs Providers.MySql.fs Providers.Odbc.fs Providers.Oracle.fs Providers.Postgresql.fs Providers.SQLite.fs SqlRuntime.QueryExpression.fs SqlRuntime.Linq.fs SqlRuntime.Async.fs SqlRuntime.DataContext.fs SqlDesignTime.fs""" ]

// --------------------------------------------------------------------------------------
// END TODO: The rest of the file includes standard build steps
// --------------------------------------------------------------------------------------


let rootDir = "perftests"
let compilerDir = Path.GetFullPath "./compiler"

Target "Clean" (fun _ ->
    CleanDirs [ rootDir; compilerDir ]
)

Target "CleanDocs" (fun _ ->
    CleanDirs ["docs/output"]
)

let projectDir name = Path.Combine(rootDir,name) |> FullName

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
                        info.Arguments <- additionalFlags + " " + args
                        info.WorkingDirectory <- Path.Combine(projectDir name,dir)) System.TimeSpan.MaxValue
                if result <> 0 then failwithf "Error during compile test of %s" name
                times.Add((name,run),sw.ElapsedMilliseconds)

        allTimings.Add(hash,times)

    let log = System.Collections.Generic.List<_>()
    
    for hash,times in allTimings do
        log.Add <| sprintf "Compiler: %s" hash
        let timesPerProject =
            times 
            |> Seq.groupBy (fun kv -> kv.Key |> fst)
            |> Seq.map (fun (name,times) -> name,times |> Seq.sumBy (fun kv -> kv.Value))
            |> Seq.toList

        timesPerProject
        |> Seq.iter (fun (name,runtime) -> log.Add <| sprintf  "   Project: %s %Ams" name (float (runtime / int64 runs)))

        tracefn ""
        timesPerProject
        |> Seq.sumBy (fun (_,runtime) -> runtime)
        |> fun runtime -> log.Add <| sprintf  "   Average Run: %Ams" (float (runtime / int64 runs))


    for line in log do
        tracefn "%s" line

    match buildServer with
    | AppVeyor -> File.WriteAllLines("TestResults.txt",log)
    | _ -> ()

)


Target "All" DoNothing
Target "CleanPerfTests" DoNothing

"Clean"
  ==> "CloneProjects"
  ==> "BuildProjects"
  ==> "CleanPerfTests"
  ==> "All"

"RunPerfTests"
  ==> "CleanPerfTests"
  
RunTargetOrDefault "All"
