@echo off
powershell ".\Create-DependencyGraph.ps1 ..\..\..\src | Out-File Dependencies.dot -encoding ascii"
dot -o Dependencies.pdf -Tpdf Dependencies.dot
IF %ERRORLEVEL% NEQ 0 PAUSE
