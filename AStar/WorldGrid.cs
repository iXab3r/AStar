using System;
using System.Drawing;
using AStar.Collections.MultiDimensional;
using AStar.Scaffolding;
using CommunityToolkit.HighPerformance;

namespace AStar;

/// <summary>
///     A world grid consisting of integers where a closed cell is represented by 0
/// </summary>
public class WorldGrid : AGrid<byte>, IWorldAGrid
{
    /// <summary>
    ///     Creates a new world with values set from the provided 2d array.
    ///     Height will be first dimension, and Width will be the second,
    ///     e.g [4,2] will have a height of 4 and a width of 2.
    /// </summary>
    /// <param name="worldArray">A 2 dimensional array of short where 0 indicates a closed node</param>
    public WorldGrid(byte[,] worldArray) : base(new Memory2D<byte>(worldArray))
    {
    }

    public WorldGrid(Size size) : base(size)
    {
    }
    
    public WorldGrid(Memory2D<byte> data) : base(data)
    {
    }
    
    public WorldGrid(ReadOnlySpan2D<byte> data) : base(new Size(data.Width, data.Height))
    {
        this.CopyFrom(data);
    }
}