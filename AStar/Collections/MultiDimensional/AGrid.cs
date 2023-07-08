using System;
using System.Collections.Generic;
using System.Drawing;
using CommunityToolkit.HighPerformance;

namespace AStar.Collections.MultiDimensional;

public class AGrid<T> : IModelAGrid<T>
{
    public AGrid(Size size) : this (new Memory2D<T>(new T[size.Height, size.Width]))
    {
    }
    
    public AGrid(Memory2D<T> data)
    {
        Data = data;
        Size = new Size(Width, Height);
    }
    
    public Memory2D<T> Data { get; }

    public int Height => Data.Height;

    public int Width => Data.Width;

    public T this[Point point]
    {
        get => this[point.Y, point.X];
        set => this[point.Y, point.X] = value;
    }

    public T this[Position position]
    {
        get => Data.Span[position.Row, position.Column];
        set => Data.Span[position.Row, position.Column] = value;
    }

    public Size Size { get; }

    public T this[int row, int column]
    {
        get => Data.Span[row, column];
        set => Data.Span[row, column] = value;
    }
}