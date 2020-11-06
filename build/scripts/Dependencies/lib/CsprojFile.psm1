class CsprojFile {

    [String]$FileName

    hidden [Xml]$csprojXml

    CsprojFile ([String]$FileName) {
        $this.FileName = $FileName
        $this.csprojXml = (Get-Content -Path $this.FileName)
    }

    [String[]] GetProjectReferences() {
        $projectReferences = $this.csprojXml.SelectNodes("//ProjectReference") |
            ForEach-Object { $_.include }

        if ($null -eq $projectReferences) {
            return @() 
        } else { 
            return $projectReferences 
        }
    }
}
