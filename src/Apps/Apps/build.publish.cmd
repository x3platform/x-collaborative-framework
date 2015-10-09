@echo off

set SolutionDir=..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set SrcDir=%SolutionDir%src\

set TargetWebSiteDir=%SrcDir%WebSite\1.0.0\

set TargetIBatisResourceDir=%SrcDir%WebSite\1.0.0\resources\ibatis\Apps\

set ProjectDir=%1

set TargetDir=%2

set TargetName=%3

set TargetDBProvider=%4

echo Deploy project files - %TargetName%

call :CopyTargetFiles

@rem if exist "%TargetWebSiteDir%config\" call:CopyConfigFiles

@rem if exist "%TargetIBatisResourceDir%" call:CopyIBatisFiles

@rem =========================================================
@rem Build Project Function
@rem =========================================================

:CopyTargetFiles

	if exist "%TargetDir%%TargetName%.dll" copy "%TargetDir%%TargetName%.dll" %BinDir% /y
	if exist "%TargetDir%%TargetName%.pdb" copy "%TargetDir%%TargetName%.pdb" %BinDir% /y
	if exist "%TargetDir%%TargetName%.xml" copy "%TargetDir%%TargetName%.xml" %BinDir% /y
	
goto :EOF

:CopyConfigFiles			
	
	@REM attrib "%TargetWebSiteDir%config\*.config" -r
	
	xcopy "%ProjectDir%config\*.config" "%TargetWebSiteDir%config\" /y
    
    if exist "%ProjectDir%config\%TargetDBProvider%\*.config" xcopy "%ProjectDir%config\%TargetDBProvider%\*.config" "%TargetWebSiteDir%config\" /y
	
	@REM attrib "%TargetWebSiteDir%config\*.config" +r
	    
goto :EOF       

:CopyIBatisFiles
	
	@REM attrib "%TargetIBatisResourceDir%*.xml" -r
	
	xcopy "%ProjectDir%DAL\%TargetDBProvider%\*.xml" "%TargetIBatisResourceDir%" /y

	@REM attrib "%TargetIBatisResourceDir%*.xml" +r

goto :EOF