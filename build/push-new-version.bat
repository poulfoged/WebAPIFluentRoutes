@echo off
del *.nupkg
..\source\packages\NuGet.CommandLine.2.8.5\tools\nuget pack ..\source\WebAPIFluentRoutes\WebAPIFluentRoutes.csproj
..\source\packages\NuGet.CommandLine.2.8.5\tools\nuget push *.nupkg
