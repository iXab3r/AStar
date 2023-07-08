using System.Collections.Generic;
using AStar.Collections.MultiDimensional;
using CommunityToolkit.HighPerformance;

namespace AStar.Scaffolding;

public static class ModelAGridExtensions
{
    public static void CopyFrom<T>(this IModelAGrid<T> grid, ReadOnlySpan2D<T> data)
    {
        data.CopyTo(grid.Data.Span);
    }
    
    public static IEnumerable<Position> GetSuccessorPositions<T>(this IModelAGrid<T> grid, Position node, bool optionsUseDiagonals = false)
    {
        var offsets = GridOffsetUtils.GetOffsets(optionsUseDiagonals);
        foreach (var neighbourOffset in offsets)
        {
            var successorRow = node.Row + neighbourOffset.row;

            if (successorRow < 0 || successorRow >= grid.Size.Height)
            {
                continue;
            }

            var successorColumn = node.Column + neighbourOffset.column;

            if (successorColumn < 0 || successorColumn >= grid.Size.Width)
            {
                continue;
            }

            yield return new Position(successorRow, successorColumn);
        }
    }
}