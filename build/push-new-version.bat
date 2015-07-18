@echo off
del *.nupkg
tools\nuget pack ..\source\WebAPIFluentRoutes\WebAPIFluentRoutes.csproj
tools\nuget push *.nupkg
