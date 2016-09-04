# FSharpPerf

This project contains a set of performance test scripts for the F# compiler.

This script will try to measure compile times of different projects for different versions of the F# compiler.

The idea is that you clone https://github.com/Microsoft/visualfsharp somewhere to your local machine and then make change in the compiler.
This script will help you to measure performance impact of the change.

## Usage

* Change properties in build.fsx like compiler location and commit hashes you are interested in
* Run "build.cmd BuildProjects"

Now all projects are cloned to ./perftests/ and prepared for the test runs.

* Now run "build.cmd RunPerfTests"

This will clone the compiler project to ./compiler/ and checkout the first hash. Then it will compile the compiler itself and use it to compile the test projects.

At the end you will get a report:



## Maintainer(s)

- [@forki](https://github.com/forki)
