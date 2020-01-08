. $PSScriptRoot\config.ps1

Write-Host "$msbuildexe"

Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.sln | foreach {
    & $msbuildexe $_.FullName
}