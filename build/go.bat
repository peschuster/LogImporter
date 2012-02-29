@ECHO OFF

SET msbuild="%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

IF '%1'=='' (SET target="Build") ELSE (SET target=%1)
IF '%2'=='' (SET configuration="Release") ELSE (SET configuration=%2)

%msbuild% .\build.proj /t:%target% /property:Configuration=%configuration%