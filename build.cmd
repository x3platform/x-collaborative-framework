@echo off

@rem set /p targetFramework=Please Enter(net-3.5 OR net-4.0 OR mono-3.5)£º
set targetFramework=%~1

if "%targetFramework%" equ "net-3.5" goto NET35
if "%targetFramework%" equ "net-4.0" goto NET40
if "%targetFramework%" equ "mono-2.0" goto MONO20
if "%targetFramework%" equ "mono-3.5" goto MONO35
if "%targetFramework%" equ "mono-4.0" goto MONO40
goto DEFAULT

:DEFAULT
nant -buildfile:default.build -logfile:build.txt
goto end
:NET35
nant -buildfile:default.build -t:%targetFramework% -logfile:build.net-3.5.txt
goto end
:NET40
nant -buildfile:default.build -t:%targetFramework% -logfile:build.net-4.0.txt
goto end
:MONO20
nant -buildfile:default.build -t:%targetFramework% -logfile:build.mono-2.0.txt
goto end
:MONO35
nant -buildfile:default.build -t:%targetFramework% -logfile:build.mono-3.5.txt
goto end
:MONO40
nant -buildfile:default.build -t:%targetFramework% -logfile:build.mono-4.0.txt
goto end
:END
