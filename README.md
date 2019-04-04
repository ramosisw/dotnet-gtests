# dotnet-gtests
[![Build Status](https://img.shields.io/travis/ramosisw/dotnet-gtests/master.svg?style=flat-square&logo=travis)](https://travis-ci.org/ramosisw/dotnet-gtests)
[![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square&logo=read-the-docs)](https://github.com/ramosisw/dotnet-gtests/blob/master/LICENSE)
[![NuGet Version](https://img.shields.io/nuget/v/dotnet-gtests.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/dotnet-gtests/)
[![NuGet Download](https://img.shields.io/nuget/dt/dotnet-gtests.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/dotnet-gtests/)

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
  -m, --gmethods        Public methods must be included in the test class. (default true).
  -o, --output-dir      The ouput folder where be add tests classes, relative to <PROJECT>. (default is root on <PROJECT>)
```


# Examples

<p align="center"><img src="/img/dotnet-gtests.gif?raw=true"/></p>

<p align="center"><img src="/img/dotnet-gtests-already-exists.gif?raw=true"/></p>
