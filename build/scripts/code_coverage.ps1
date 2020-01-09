. $PsScriptRoot\config.ps1

$searchDirs="";
Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.Tests.dll |
    ? { $_.FullName -notmatch "obj\\" } |
    % {
        Get-ChildItem $_.Directory
        $searchDirs = $searchDirs + $_.Directory + ";"
    }

New-Item -ItemType Directory -Force -Path "$codeCoverageOutputDir"

& $opencoverconsoleexe -target:$PsScriptRoot\run_tests.cmd  -filter:"+[$codeCoverageNamespacePrefix.*]* -[*.Tests]*" -register:user -searchdirs:"$searchDirs" -output:"$codeCoverageOutputDir\\results.xml"

if (Test-Path 'env:APPVEYOR_BUILD_NUMBER') {
    & $coverallsconsoleexe  --opencover -i "$codeCoverageOutputDir\\results.xml" --serviceNumber $env:APPVEYOR_BUILD_NUMBER
}