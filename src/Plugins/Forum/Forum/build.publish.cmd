@echo off

set SolutionDir=..\..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set SrcDir=%SolutionDir%src\

set TargetWebSiteDir=%SrcDir%WebSite\1.0.0\

set TargetIBatisResourceDir=%TrunkDir%WebSite\1.0.0\resources\ibatis\Forum\

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
	
	@rem attrib "%TargetWebSiteDir%config\*.config" -r
	
	xcopy "%ProjectDir%config\*.config" "%TargetWebSiteDir%config\" /y
    
    if exist "%ProjectDir%config\%TargetDBProvider%\*.config" xcopy "%ProjectDir%config\%TargetDBProvider%\*.config" "%TargetWebSiteDir%config\" /y
	
	@rem attrib "%TargetWebSiteDir%config\*.config" +r
	    
goto :EOF       

:CopyIBatisFiles
	
	@rem attrib "%TargetIBatisResourceDir%*.xml" -r
	
	xcopy "%ProjectDir%DAL\%TargetDBProvider%\*.xml" "%TargetIBatisResourceDir%" /y

	@rem attrib "%TargetIBatisResourceDir%*.xml" +r

goto :EOF