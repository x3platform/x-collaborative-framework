@echo off

set BinDir=E:\Workspace\X3Platform\bin\

set TrunkDir=E:\Workspace\X3Platform\trunk\

set TargetWebSiteDir=%TrunkDir%WebSite\1.0.0\

set ProjectDir=%1

set TargetDir=%2

set TargetName=%3

echo %1

echo "开始部署文件"

copy "%TargetDir%%TargetName%.dll" "%BinDir%" /y
copy "%TargetDir%%TargetName%.pdb" "%BinDir%" /y
copy "%TargetDir%%TargetName%.xml" "%BinDir%" /y

if exist "%TargetWebSiteDir%config\" attrib "%TargetWebSiteDir%config\*.config" -r

if exist "%TargetWebSiteDir%config\" xcopy "%ProjectDir%config\*.config" "%TargetWebSiteDir%config\" /y

if exist "%TargetWebSiteDir%resources\ibatis\Apps\Security\" attrib "%TargetWebSiteDir%resources\ibatis\Apps\Security\*.xml" -r

if exist "%TargetWebSiteDir%resources\ibatis\Apps\Security\" xcopy "%ProjectDir%DAL\IBatis\*.xml" "%TargetWebSiteDir%resources\ibatis\Apps\Security\" /y