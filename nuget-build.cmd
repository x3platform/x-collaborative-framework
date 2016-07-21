@echo off

set nuget=tools\nuget\nuget.exe

@rem build
@rem call build.cmd

@rem read version
for /f %%a in ('type dist\nuget.version') do SET version=%%a

@rem nuget pack nuget.nuspec
%nuget% pack dist\nuget.nuspec -OutputDirectory dist -Version %version%

@rem upload nuget.org
@rem %nuget% push dist\x-collaborative-framework.%version%.nupkg
