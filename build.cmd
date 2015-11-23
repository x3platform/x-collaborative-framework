@echo off
setlocal
for %%a in (%*) do echo "%%a" | findstr /C:"mono">nul && set buildtool=xbuild.bat
if not defined buildtool for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v*"') do @if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe

if not defined buildtool (echo no MSBuild.exe or xbuild was found>&2 & exit /b 42)

@REM call "tools/nuget.exe" restore nunit.sln
@REM if errorlevel 1 (echo NuGet restore failed.>&2 & exit /b 1)

@REM if defined buildtool "%buildtool%" %*
if defined buildtool "%buildtool%" ./src/makefile.msbuild
