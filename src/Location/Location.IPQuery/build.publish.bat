@echo off

set SolutionDir=..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set TrunkDir=%SolutionDir%trunk\

set TargetWebSiteDir=%TrunkDir%WebSite\1.0.0\

set TargetIBatisResourceDir=%TrunkDir%WebSite\1.0.0\resources\ibatis\Location\IPQuery\

set ProjectDir=%1

set TargetDir=%2

set TargetName=%3

echo Deploy project files - %TargetName%

call :CopyTargetFiles

if exist "%TargetWebSiteDir%config\" call:CopyConfigFiles

if exist "%TargetWebSiteDir%config\" call:CopyIBatisFiles

@rem =========================================================
@rem Build Project Function
@rem =========================================================

:CopyTargetFiles

	if exist "%TargetDir%%TargetName%.dll" copy "%TargetDir%%TargetName%.dll" %BinDir% /y
	if exist "%TargetDir%%TargetName%.pdb" copy "%TargetDir%%TargetName%.pdb" %BinDir% /y
	if exist "%TargetDir%%TargetName%.xml" copy "%TargetDir%%TargetName%.xml" %BinDir% /y
	
goto :EOF

:CopyConfigFiles			
	
	attrib "%TargetWebSiteDir%config\*.config" -r
	
	xcopy "%ProjectDir%config\*.config" "%TargetWebSiteDir%config\" /y

	attrib "%TargetWebSiteDir%config\*.config" +r
	    
goto :EOF       

:CopyIBatisFiles
	
	attrib "%TargetIBatisResourceDir%*.xml" -r
	
	xcopy "%ProjectDir%DAL\IBatis\*.xml" "%TargetIBatisResourceDir%" /y

	attrib "%TargetIBatisResourceDir%*.xml" +r

goto :EOF       