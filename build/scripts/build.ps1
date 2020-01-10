. $PSScriptRoot\config.ps1

Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.sln | foreach {
    MsBuild $_.FullName
    if (! $?) { throw "Building project $_ failed" }
}