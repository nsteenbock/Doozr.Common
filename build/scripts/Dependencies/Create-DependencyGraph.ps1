using module .\lib\NuspecFile.psm1
using module .\lib\CsprojFile.psm1
using module .\lib\DirectedGraph.psm1

[CmdletBinding()]
param (
    [string] $SourceRootPath
)

begin {
    $graph = [DirectedGraph]::new()
}

process {
    [NuspecFile[]]$nuspecFiles = 
        Get-ChildItem -Path "$($SourceRootPath)" -Recurse -Filter *.nuspec | 
        Where-Object { $_.FullName -notlike "*\obj\*" } | 
        ForEach-Object { [NuspecFile]::new($_.FullName) }

    $allNuspecIds = $nuspecFiles | ForEach-Object { $_.GetId() }

    $processedItems = 0

    foreach ($nuspecFile in $nuspecFiles) {
        
        $nuspecId = $nuspecFile.GetId() 
        
        #Write-Host $nuspecId
        
        $graph.AddVertex($nuspecId)

        $nuspecDependencies = $nuspecFile.GetDependencies()

        $csprojFile = [CsprojFile]::new([Io.Path]::
            ChangeExtension($nuspecFile.FileName, ".csproj"))
        $projectReferences = $csprojFile.GetProjectReferences()
        $csprojDependencies = $projectReferences | 
            ForEach-Object { ([io.path]::GetFileNameWithoutExtension(($_))).ToLower() }

        # Package references in csproj file and dependencies in nuget package match
        $nuspecDependencies |
            Where-Object { $csprojDependencies -contains $_ } |
            ForEach-Object { $graph.AddEdge($nuspecId, $_, "green") }

        # Nuget package has dependency on package not referenced in csproj file 
        $nuspecDependencies |
            Where-Object { $allNuspecIds -contains $_ } |
            Where-Object { -not ($csprojDependencies -contains $_) } |
            ForEach-Object { $graph.AddEdge($nuspecId, $_, "red") }

        # Nuget package is missing dependency for package referenced in csproj file
        $csprojDependencies |
            Where-Object { -not ($nuspecDependencies -contains $_) } |
            ForEach-Object { $graph.AddEdge($nuspecId, $_, "orange") }

        $processedItems++
        
        $progressParameters = @{
            Activity = "Processing Nuspec-Files"
            Status = $nuspecId
            PercentComplete = ($processedItems*100/$nuspecFiles.Count)
        }
        Write-Progress @progressParameters
    }
}

end {
    Write-Output $graph.GetDotFile()
}