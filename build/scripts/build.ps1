Get-ChildItem $PsScriptRoot\..\src -Recurse -Filter *.sln | foreach {
    msbuild $_.FullName
}