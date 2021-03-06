@echo off

set SolutionDir=..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set SrcDir=%SolutionDir%src\

set TargetWebSiteDir=%SrcDir%WebSite\1.0.0\

set TargetIBatisResourceDir=%SrcDir%WebSite\1.0.0\resources\ibatis\Training\

set TrainingDir=%1

set TargetDir=%2

set TargetName=%3

set TargetDBProvider=%4

echo Deploy Training files - %TargetName%

call :CopyTargetFiles

@rem if exist "%TargetWebSiteDir%config\" call:CopyConfigFiles

@rem if exist "%TargetIBatisResourceDir%" call:CopyIBatisFiles

@rem =========================================================
@rem Build Training Function
@rem =========================================================

:CopyTargetFiles

	if exist "%TargetDir%%TargetName%.dll" copy "%TargetDir%%TargetName%.dll" %BinDir% /y
	if exist "%TargetDir%%TargetName%.pdb" copy "%TargetDir%%TargetName%.pdb" %BinDir% /y
	if exist "%TargetDir%%TargetName%.xml" copy "%TargetDir%%TargetName%.xml" %BinDir% /y
	
goto :EOF

:CopyConfigFiles
	
	@rem attrib "%TargetWebSiteDir%config\*.config" -r
	
	xcopy "%TrainingDir%config\*.config" "%TargetWebSiteDir%config\" /y
    
    if exist "%TrainingDir%config\%TargetDBProvider%\*.config" xcopy "%TrainingDir%config\%TargetDBProvider%\*.config" "%TargetWebSiteDir%config\" /y
	
	@rem attrib "%TargetWebSiteDir%config\*.config" +r
	    
goto :EOF       

:CopyIBatisFiles
	
	@rem attrib "%TargetIBatisResourceDir%*.xml" -r
	
	xcopy "%TrainingDir%DAL\%TargetDBProvider%\*.xml" "%TargetIBatisResourceDir%" /y

	@rem attrib "%TargetIBatisResourceDir%*.xml" +r

goto :EOF