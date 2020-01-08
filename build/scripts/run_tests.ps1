. $PsScriptRoot\config.ps1

Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.Tests.dll | foreach {
	& $xunitconsoleexe $_.FullName
}

