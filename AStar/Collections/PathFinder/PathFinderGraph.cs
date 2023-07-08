using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AStar.Collections.MultiDimensional;
using AStar.Collections.PriorityQueue;
using AStar.Scaffolding;

namespace AStar.Collections.PathFinder;

internal class PathFinderGraph : IModelAGraph<PathFinderNode>
{
    private readonly bool allowDiagonalTraversal;
    private readonly AGrid<PathFinderNode> internalGrid;
    private readonly SimplePriorityQueue<PathFinderNode> open = new(new ComparePathFinderNodeByFValue());

    public PathFinderGraph(Size size, bool allowDiagonalTraversal)
    {
        this.allowDiagonalTraversal = allowDiagonalTraversal;
        internalGrid = new AGrid<PathFinderNode>(size);
        Initialise();
    }

    public bool HasOpenNodes => open.Count > 0;

    public IEnumerable<PathFinderNode> GetSuccessors(PathFinderNode node)
    {
        return internalGrid
            .GetSuccessorPositions(node.Position, allowDiagonalTraversal)
            .Select(successorPosition => internalGrid[successorPosition]);
    }

    public PathFinderNode GetParent(PathFinderNode node)
    {
        return internalGrid[node.ParentNodePosition];
    }

    public void OpenNode(PathFinderNode node)
    {
        internalGrid[node.Position] = node;
        open.Push(node);
    }

    public PathFinderNode GetOpenNodeWithSmallestF()
    {
        return open.Pop();
    }

    private void Initialise()
    {
        for (var row = 0; row < internalGrid.Height; row++)
        {
            for (var column = 0; column < internalGrid.Width; column++)
            {
                internalGrid[row, column] = new PathFinderNode(new Position(row, column),
                    0,
                    0,
                    default);
            }
        }

        open.Clear();
    }
}