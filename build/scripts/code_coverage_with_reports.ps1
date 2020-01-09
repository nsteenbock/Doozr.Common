. $PSScriptRoot\config.ps1

. $PSScriptRoot\code_coverage.ps1

& $reportgeneratorexe -reports:"$codeCoverageOutputDir\\results.xml" -targetdir:"$codeCoverageOutputDir\\coveragereport" -historydir:"$codeCoverageOutputDir\\coveragehistory"