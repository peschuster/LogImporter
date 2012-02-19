@ECHO OFF

IF "%1" == "" (
	C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe .\build.proj
) ELSE (
	C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe .\build.proj /t:%*
)