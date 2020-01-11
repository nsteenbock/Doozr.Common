. $PSScriptRoot\config.ps1

nuget install OpenCover -Version $opencoverversion -OutputDirectory $PsScriptRoot\..\tools
nuget install coveralls.net -Version $coverallsversion -OutputDirectory $PsScriptRoot\..\tools
nuget install xunit.runner.console -Version $xunitversion -OutputDirectory $PsScriptRoot\..\tools
nuget install ReportGenerator -Version $reportgeneratorversion -OutputDirectory $PsScriptRoot\..\tools

Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.sln | ForEach-Object {
    nuget restore $_.FullName
}