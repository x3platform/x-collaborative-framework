@echo off

set SolutionDir=..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set TrunkDir=%SolutionDir%trunk\

set TargetWebSiteDir=%TrunkDir%WebSite\1.0.0\

set ProjectDir=%1

set TargetDir=%2

set TargetName=%3

set TargetDBProvider=%4

echo Deploy project files - %TargetName%

echo %BinDir%
 
if not exist "%BinDir%" md %BinDir% 

call :CopyTargetFiles

@rem =========================================================
@rem Build Project Function
@rem =========================================================

:CopyTargetFiles

	if exist "%TargetDir%%TargetName%.dll" copy "%TargetDir%%TargetName%.dll" %BinDir% /y
	if exist "%TargetDir%%TargetName%.pdb" copy "%TargetDir%%TargetName%.pdb" %BinDir% /y
    if exist "%TargetDir%%TargetName%.xml" copy "%TargetDir%%TargetName%.xml" %BinDir% /y

goto :EOF
