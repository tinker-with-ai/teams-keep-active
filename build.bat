@echo off
rem Builds KeepAwake.exe with the C# compiler included in Windows.
"%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /target:winexe /out:"%~dp0KeepAwake.exe" "%~dp0KeepAwake.cs"
