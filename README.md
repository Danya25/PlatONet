# PlatONet
PlatONet PlatON.dotnet SDK。SDK.net standard 2.0。

PlatONet BouncyCastle Nethereum。
# Tutorial

## 1. Compile from source code

### 1.1. Use .NET Core 3.1

Install it from docs.microsoft.com：[Install .NET on Windows, Linux, and macOS](https://docs.microsoft.com/en-us/dotnet/core/install/)。

### 1.2. Package restore

The function of package restoration is to download the package referenced by the project from NuGet, which may take a long time depending on the network environment。

If you open this project using Visual Studio 2019, package restore will be automatically restored after opening the project. If the package restore is not automatically done, you can right-click the solution in Solution Explorer and select "Restore NuGet Package (R)" to restore the package.

You can also use the command line to run the `dotnet restore` command in the project root directory to explicitly restore the dependencies of the NuGet package. But in most cases, it is not necessary to use the `dotnet restore` command explicitly, because when running subsequent commands, NuGet restore will be run implicitly if necessary.

### 1.3. Generate project

If you use Visual Studio 2019 as a development tool, you can use the build->project** to generate the project, or you can use `dotnet build` to generate the project on the command line.

### 1.4. Examples

To help everyone understand how to use PlatONet faster, an example was specially written. It contains basic operations such as query information, transfer, smart contract deployment, smart contract call, etc., and you can explore and use it yourself.
