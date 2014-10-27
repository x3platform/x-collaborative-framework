@echo off

set SolutionDir=..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set SrcDir=%SolutionDir%src\

set TargetWebSiteDir=%SrcDir%WebSite\1.0.0\

set ProjectDir=%1

set TargetDir=%2

set TargetName=%3

echo Deploy project files - %TargetName%

copy "%TargetDir%%TargetName%.dll" "%BinDir%" /y
copy "%TargetDir%%TargetName%.pdb" "%BinDir%" /y