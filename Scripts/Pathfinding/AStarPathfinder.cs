namespace AutoBattleRPG.Scripts.Pathfinding;

public class AStarPathfinder
{
    private readonly PathMap _pathMap;
    private readonly SortedSet<PathNode> _openList = new (new PathNodeComparer());
    private readonly HashSet<PathNode> _closedList = new ();
    private readonly IAStarInfoProviderDelegate _infoProvider;

    public AStarPathfinder(int width, int height, IAStarInfoProviderDelegate infoProvider)
    {
        _pathMap = new PathMap(width, height);
        _infoProvider = infoProvider;
    }

    /// <summary>
    ///     Find path from node at start to node at end
    /// </summary>
    /// <returns> Path from start to end, null if none found. </returns>
    public List<(int x, int y)>? FindPath((int x, int y) start, (int x, int y) end)
    {
        PathNode startNode = _pathMap[start.x, start.y];
        PathNode targetNode = _pathMap[end.x, end.y];
        
        Reset();
        
        _openList.Add(startNode);

        startNode.GCost = 0;
        startNode.HCost = _infoProvider.Distance(startNode, targetNode);
        
        while(_openList.Count > 0)
        {
            PathNode currentNode = _openList.First();

            if(currentNode == targetNode) return RetrievePath(startNode, targetNode);

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach(PathNode neighbor in _infoProvider.GetNeighborhood(currentNode, targetNode))
            {
                if(_closedList.Contains(neighbor)) continue;
                
                int newGcost = currentNode.GCost + 1;
                if (newGcost >= neighbor.GCost) continue;
                
                neighbor.PrevNode = currentNode;
                neighbor.GCost = newGcost;
                neighbor.HCost = _infoProvider.Distance(neighbor, targetNode);

                _openList.Add(neighbor);
            }
        }

        // No path found
        return null;
    }
    
    private List<(int x, int y)> RetrievePath(PathNode startNode, PathNode endNode)
    {
        List<(int x, int y)> path = new () { (endNode.X, endNode.Y) };

        PathNode currentNode = endNode;
        
        while(currentNode.PrevNode != startNode && currentNode.PrevNode != null)
        {
            path.Add((currentNode.PrevNode.X, currentNode.PrevNode.Y));
            currentNode = currentNode.PrevNode;
        }

        path.Reverse();
        return path;
    }

    private void Reset()
    {
        _openList.Clear();
        _closedList.Clear();

        for(int x = 0; x < _pathMap.Width; x++)
        {
            for(int y = 0; y < _pathMap.Height; y++)
            {
                PathNode currentNode = _pathMap[x, y];
                currentNode.GCost = int.MaxValue;
                currentNode.PrevNode = null;
            }
        }
    }
}