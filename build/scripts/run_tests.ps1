. $PsScriptRoot\config.ps1

Get-ChildItem $PsScriptRoot\..\..\src -Recurse -Filter *.Tests.dll | 
	? {$_.FullName -notmatch "obj\\"} | 
	% {
	& $xunitconsoleexe $_.FullName
}

