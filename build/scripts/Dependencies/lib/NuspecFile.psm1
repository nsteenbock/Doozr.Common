class NuspecFile {
    
    [String]$FileName

    hidden [Xml]$nuspecXml

    NuspecFile ([String]$FileName) {
        $this.FileName = $FileName
        $this.nuspecXml = (Get-Content -Path $this.FileName)
    }

    [String] GetId(){
       return $this.nuspecXml.package.metadata.id
    }

    [String[]] GetDependencies(){
        $dependencyNodes = $this.nuspecXml.package.metadata.dependencies.group.dependency
        $dependencies = @()
        foreach ($dependencyNode in $dependencyNodes)
        {
            $dependencyId = $dependencyNode.GetAttribute("id")
            $dependencies += $dependencyId
        }

        return $dependencies
    }
}
