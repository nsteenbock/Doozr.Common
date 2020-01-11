. $PSScriptRoot\config.ps1

Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.sln | ForEach-Object {
    MsBuild $_.FullName
    if (! $?) { exit 1 }
}

Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.nuspec | 
    Where-Object { $_.FullName -notmatch "obj\\" } |
    ForEach-Object {
        nuget pack $_.FullName -Version $buildNumber -OutputDirectory $PsScriptRoot\..\nuget-packages
    }