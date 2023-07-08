using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AStar.Collections.MultiDimensional;
using AStar.Collections.PathFinder;
using AStar.Heuristics;
using AStar.Options;

namespace AStar;

public class PathFinder : IFindAPath
{
    private const int ClosedValue = 0;
    private const int DistanceBetweenNodes = 1;
    private readonly ICalculateHeuristic heuristic;
    private readonly PathFinderOptions options;
    private readonly IWorldAGrid world;

    public PathFinder(IWorldAGrid worldGrid, PathFinderOptions pathFinderOptions = null)
    {
        world = worldGrid ?? throw new ArgumentNullException(nameof(worldGrid));
        options = pathFinderOptions ?? new PathFinderOptions();
        heuristic = HeuristicFactory.Create(options.HeuristicFormula);
    }

    /// <inheritdoc />
    public Point[] FindPath(Point start, Point end)
    {
        return FindPath(new Position(start.Y, start.X), new Position(end.Y, end.X))
            .Select(position => new Point(position.Column, position.Row))
            .ToArray();
    }

    /// <inheritdoc />
    public Position[] FindPath(Position start, Position end)
    {
        var nodesVisited = 0;
        IModelAGraph<PathFinderNode> graph = new PathFinderGraph(world.Size, options.UseDiagonals);

        var startNode = new PathFinderNode(start, 0, 2, start);
        graph.OpenNode(startNode);

        while (graph.HasOpenNodes)
        {
            var q = graph.GetOpenNodeWithSmallestF();

            if (q.Position == end)
            {
                return OrderClosedNodesAsArray(graph, q);
            }

            if (nodesVisited > options.SearchLimit)
            {
                return Array.Empty<Position>();
            }

            foreach (var successor in graph.GetSuccessors(q))
            {
                if (world[successor.Position] == ClosedValue)
                {
                    continue;
                }

                var newG = q.G + DistanceBetweenNodes;

                if (options.PunishChangeDirection)
                {
                    newG += CalculateModifierToG(q, successor, end);
                }

                var updatedSuccessor = new PathFinderNode(
                    successor.Position,
                    newG,
                    heuristic.Calculate(successor.Position, end),
                    q.Position);

                if (BetterPathToSuccessorFound(updatedSuccessor, successor))
                {
                    graph.OpenNode(updatedSuccessor);
                }
            }

            nodesVisited++;
        }

        return Array.Empty<Position>();
    }

    private int CalculateModifierToG(PathFinderNode q, PathFinderNode successor, Position end)
    {
        if (q.Position == q.ParentNodePosition)
        {
            return 0;
        }

        var gPunishment = Math.Abs(successor.Position.Row - end.Row) + Math.Abs(successor.Position.Column - end.Column);

        var successorIsVerticallyAdjacentToQ = successor.Position.Row - q.Position.Row != 0;

        if (successorIsVerticallyAdjacentToQ)
        {
            var qIsVerticallyAdjacentToParent = q.Position.Row - q.ParentNodePosition.Row == 0;
            if (qIsVerticallyAdjacentToParent)
            {
                return gPunishment;
            }
        }

        var successorIsHorizontallyAdjacentToQ = successor.Position.Row - q.Position.Row != 0;

        if (successorIsHorizontallyAdjacentToQ)
        {
            var qIsHorizontallyAdjacentToParent = q.Position.Row - q.ParentNodePosition.Row == 0;
            if (qIsHorizontallyAdjacentToParent)
            {
                return gPunishment;
            }
        }

        if (!options.UseDiagonals)
        {
            return 0;
        }

        var successorIsDiagonallyAdjacentToQ = successor.Position.Column - successor.Position.Row == q.Position.Column - q.Position.Row;
        if (!successorIsDiagonallyAdjacentToQ)
        {
            return 0;
        }

        var qIsDiagonallyAdjacentToParent = q.Position.Column - q.Position.Row == q.ParentNodePosition.Column - q.ParentNodePosition.Row
                                            && IsStraightLine(q.ParentNodePosition, q.Position, successor.Position);
        if (!qIsDiagonallyAdjacentToParent)
        {
            return 0;
        }

        return gPunishment;

    }

    private static bool IsStraightLine(Position a, Position b, Position c)
    {
        // area of triangle == 0
        return (a.Column * (b.Row - c.Row) + b.Column * (c.Row - a.Row) + c.Column * (a.Row - b.Row)) / 2 == 0;
    }

    private static bool BetterPathToSuccessorFound(PathFinderNode updateSuccessor, PathFinderNode currentSuccessor)
    {
        return !currentSuccessor.HasBeenVisited ||
               (currentSuccessor.HasBeenVisited && updateSuccessor.F < currentSuccessor.F);
    }

    private static Position[] OrderClosedNodesAsArray(IModelAGraph<PathFinderNode> graph, PathFinderNode endNode)
    {
        var path = new Stack<Position>();

        var currentNode = endNode;

        while (currentNode.Position != currentNode.ParentNodePosition)
        {
            path.Push(currentNode.Position);
            currentNode = graph.GetParent(currentNode);
        }

        path.Push(currentNode.Position);

        return path.ToArray();
    }
}