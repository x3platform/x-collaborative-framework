@echo off

set SolutionDir=..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set ProjectDir=%1

set TargetDir=%2

set TargetName=%3

echo Deploy project files - %TargetName%

call :CopyTargetFiles

@rem if exist "%ServicePublishDir%" call :CopyServicePublishFiles

@rem =========================================================
@rem Build Project Function
@rem =========================================================

:CopyTargetFiles

	if exist "%TargetDir%%TargetName%.dll" copy "%TargetDir%%TargetName%.dll" %BinDir% /y
	if exist "%TargetDir%%TargetName%.pdb" copy "%TargetDir%%TargetName%.pdb" %BinDir% /y
	if exist "%TargetDir%%TargetName%.xml" copy "%TargetDir%%TargetName%.xml" %BinDir% /y
		
goto :EOF

:CopyServicePublishFiles

	copy "%ProjectDir%console.*.bat" "%ServicePublishDir%" /y
	
	if exist "%TargetDir%%TargetName%.dll" copy "%TargetDir%%TargetName%.dll" %BinDir% /y
	if exist "%TargetDir%%TargetName%.pdb" copy "%TargetDir%%TargetName%.pdb" %BinDir% /y
	if exist "%TargetDir%%TargetName%.xml" copy "%TargetDir%%TargetName%.xml" %BinDir% /y
	
	call "%ServicePublishDir%console.stop.services.bat"

	xcopy "%TargetDir%*.*" "%ServicePublishDir%" /y
	
	@rem call "%ServicePublishDir%console.start.services.bat"
	
	del "%ServicePublishDir%*.vshost.exe"
	del "%ServicePublishDir%*.vshost.exe.*"

goto :EOF
