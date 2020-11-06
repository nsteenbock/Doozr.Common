class DirectedGraph {
    
    [String[]]$Vertexes = @()

    [Edge[]]$Edges = @()

    [void] AddVertex([String]$label) {
        $this.Vertexes += $label
    }

    [void] AddEdge([String]$sourceVertex, [String]$targetVertex, [String]$color) {
        $edge = [Edge]::new($sourceVertex, $targetVertex, $color)
        $this.Edges += $edge
    }

    [String] GetDotFile() {
        $dotFile = [System.Text.StringBuilder]::new()
        
        $dotFile.AppendLine("digraph G {")
        $dotFile.AppendLine("   rankdir=""LR"";")
        $dotFile.AppendLine("   node [shape=box];")
        $dotFile.AppendLine("")
        $this.AddVertexesToDotFile($dotFile)
        $this.AddEdgesToDotFile($dotFile)
        $dotFile.AppendLine("}")
        return $dotFile.ToString()
    }

    hidden [void] AddVertexesToDotFile([System.Text.StringBuilder]$builder) {
        foreach($vertex in $this.Vertexes) {
            $builder.AppendLine("   ""$($vertex)""")
        }
    }

    hidden [void] AddEdgesToDotFile([System.Text.StringBuilder]$builder) {
        foreach($edge in $this.Edges) {
            $builder.AppendLine("   ""$($edge.SourceVertex)"" -> ""$($edge.TargetVertex)""" +
                "[style=bold,color=$($edge.Color)];")
        }
    }
}

class Edge {

    [String]$SourceVertex

    [String]$TargetVertex

    [String]$Color

    Edge([String]$sourceVertex, [String]$targetVertex, [String]$color) {
        $this.SourceVertex = $sourceVertex
        $this.TargetVertex = $targetVertex
        $this.Color = $color
    }
}