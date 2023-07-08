using System.Collections.Generic;
using System.Drawing;
using CommunityToolkit.HighPerformance;

namespace AStar.Collections.MultiDimensional;

public interface IModelAGrid<T>
{
    Size Size { get; }
    T this[int row, int column] { get; set; }
    T this[Position position] { get; set; }
    Memory2D<T> Data { get; }
}