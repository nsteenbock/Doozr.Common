$opencoverversion="4.6.519"
$coverallsversion="0.8.0-unstable0013"
$xunitversion="2.4.0"
$reportgeneratorversion="3.1.2"

$opencoverconsoleexe="$PsScriptRoot\..\tools\OpenCover.$opencoverversion\tools\OpenCover.Console.exe"
$coverallsconsoleexe="$PsScriptRoot\..\tools\coveralls.net.$coverallsversion\tools\csmacnz.Coveralls.exe"
$xunitconsoleexe="$PsScriptRoot\..\tools\xunit.runner.console.$xunitversion\tools\net462\xunit.console.exe"
$reportgeneratorexe="$PsScriptRoot\..\tools\ReportGenerator.$reportgeneratorversion\tools\ReportGenerator.exe"

$codeCoverageOutputDir = "$PSScriptRoot\..\coverage"
$codeCoverageNamespacePrefix = "Doozr"
$codeCoverageProfilerUser = "user"
$buildNumber = "1.0.0"
$publishCodeCoverage = $FALSE


if (Test-Path 'env:APPVEYOR_BUILD_NUMBER') {
    $codeCoverageProfilerUser = "appveyor"
    $buildnumber = $env:APPVEYOR_BUILD_NUMBER
    $publishCodeCoverage = $TRUE
}