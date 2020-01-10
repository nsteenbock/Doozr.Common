. $PsScriptRoot\config.ps1

$searchDirs="";
Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.Tests.dll |
    ? { $_.FullName -notmatch "obj\\" } |
    % {
        Get-ChildItem $_.Directory
        $searchDirs = $searchDirs + $_.Directory + ";"
    }

New-Item -ItemType Directory -Force -Path "$codeCoverageOutputDir"
#Write-Host $opencoverconsoleexe -target:$PsScriptRoot\run_tests.cmd  -filter:"+[$codeCoverageNamespacePrefix.*]* -[*.Tests]*" -register:user -searchdirs:"$searchDirs" -output:"$codeCoverageOutputDir\\results.xml"
#& $opencoverconsoleexe -target:$PsScriptRoot\run_tests.cmd  -filter:"+[$codeCoverageNamespacePrefix.*]* -[*.Tests]*" -register:user -searchdirs:"$searchDirs" -output:"$codeCoverageOutputDir\\results.xml"

C:\projects\doozr-common\build\scripts\..\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:run_tests.cmd -filter:"+[Doozr.*]* -[*.Tests]*" -register:user -searchdirs:..\..\src\Doozr.Common.Utilities\Doozr.Common.Utilities.Tests\bin\Debug -output:..\coverage\results.xml

if (Test-Path 'env:APPVEYOR_BUILD_NUMBER') {
    & $coverallsconsoleexe  --opencover -i "$codeCoverageOutputDir\\results.xml" --serviceNumber $env:APPVEYOR_BUILD_NUMBER
}
