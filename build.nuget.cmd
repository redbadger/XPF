@ECHO OFF

:picnic
SET /P Push=Do you wish to push to nuget.org (y/n)?

IF /i {%Push%}=={y} (GOTO yes)
IF /i {%Push%}=={n} (GOTO no)
GOTO :picnic

:yes

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild build.proj /p:Configuration="Release" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false /t:NuGetPush

GOTO :end
:no

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild build.proj /p:Configuration="Release" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false /t:NuGet

:end
PAUSE