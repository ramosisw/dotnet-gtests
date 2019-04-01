# dotnet-gtests
[![Build Status](https://img.shields.io/travis/ramosisw/dotnet-gtests/master.svg?style=flat-square)](https://travis-ci.org/ramosisw/dotnet-gtests)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://github.com/ramosisw/dotnet-gtests/blob/master/LICENSE)
[![Source](https://img.shields.io/badge/source-GitHub-purple.svg?style=flat-square)](https://github.com/ramosisw/dotnet-gtests)
[![NuGet Version](https://badgen.net/nuget/v/dotnet-gtests)](https://www.nuget.org/packages/dotnet-gtests/)
[![NuGet Download](https://img.shields.io/nuget/dt/dotnet-gtests.svg)](https://www.nuget.org/packages/dotnet-gtests/)

A global .NET Core tool that helps to generate tests classes based on project file 

# Installation
```sh
dotnet tool install --global dotnet-gtests
```

# Usage

```
Usage:
  dotnet-gtests [options] <PROJECT>
  
Arguments:
  <PROJECT>   The project file to operate on, where be add tests classes. If a file is not specified, the command will search the current directory for one.

Options:
  -h, --help            Show command line help.
  -s, --source-project  The project file where the classes will be searched to generate tests.
  -m, --gmethods        If this option exists, public methods will be searched in the class to generate in the tests.
  -o, --output-dir      The ouput folder where be add tests classes, relative to <PROJECT>. (default is root on <PROJECT>)
```
