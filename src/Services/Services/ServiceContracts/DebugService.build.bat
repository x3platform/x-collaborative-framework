
set SVCUtil=C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\SVCUtil.exe

"%SvcUtil%" /language:c# /out:E:\Workspace\X3Platform\trunk\Services\Services\ServiceContracts\DebugServiceClient.cs /config:E:\Workspace\X3Platform\trunk\Services\Services\App.conifg http://localhost:80/api/debug
