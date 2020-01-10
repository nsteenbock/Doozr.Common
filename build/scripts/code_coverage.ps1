. $PsScriptRoot\config.ps1

$searchDirs="";
Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.Tests.dll |
    Where-Object { $_.FullName -notmatch "obj\\" } |
    ForEach-Object {
        $searchDirs = $searchDirs + $_.Directory + ";"
    }

New-Item -ItemType Directory -Force -Path "$codeCoverageOutputDir"

& $opencoverconsoleexe -target:$PsScriptRoot\run_tests.cmd `
    -filter:"+[$codeCoverageNamespacePrefix.*]* -[*.Tests]*" `
    -register:$codeCoverageProfilerUser `
    -searchdirs:"$searchDirs" `
    -output:"$codeCoverageOutputDir\\results.xml"

if ($publishCodeCoverage) {
    & $coverallsconsoleexe  --opencover -i "$codeCoverageOutputDir\\results.xml" --serviceNumber $buildNumber
}
