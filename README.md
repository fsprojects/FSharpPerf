# FSharpPerf

This project contains a set of performance test scripts for the F# compiler.

This script will try to measure compile times of different projects for different versions of the F# compiler.

The idea is that you fork https://github.com/Microsoft/visualfsharp and then make changes to the compiler.

This script will help you to measure performance impact of the change by automatically running your compiler fork on well known F# OSS projects.

## Local Usage

* Change properties in build.fsx like compiler location and commit hashes you are interested in
* Run "build.cmd BuildProjects"

Now all projects are cloned to ./perftests/ and prepared for the test runs.

* Now run "build.cmd RunPerfTests"

This will clone the compiler project to ./compiler/ and checkout the first hash. Then it will compile the compiler itself and use it to compile the test projects.

At the end you will get a report:

![Alt text](https://github.com/fsprojects/FSharpPerf/blob/master/docs/files/img/output.png "Report")

## Testing on AppVeyor

This project allows you to run compiler performance experiments on AppVeyor. For this do the following:

* Fork https://github.com/fsprojects/FSharpPerf
* Configure your F# compiler fork and the hashes to test in build.fsx
* Commit the change and send a PR

This will start the test. When it's done you will find the results in AppVeyor's build artifacts.

## Maintainer(s)

- [@forki](https://github.com/forki)
