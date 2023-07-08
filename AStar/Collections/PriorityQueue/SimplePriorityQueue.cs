using System.Collections.Generic;

namespace AStar.Collections.PriorityQueue;

internal class SimplePriorityQueue<T> : IModelAPriorityQueue<T>
{
    private readonly IComparer<T> comparer;
    private readonly List<T> innerList = new();

    public SimplePriorityQueue(IComparer<T> comparer = null)
    {
        this.comparer = comparer ?? Comparer<T>.Default;
        ;
    }

    public T this[int index]
    {
        get => innerList[index];
        set
        {
            innerList[index] = value;
            Update(index);
        }
    }

    public T Peek()
    {
        return innerList.Count > 0 ? innerList[0] : default;
    }

    public void Clear()
    {
        innerList.Clear();
    }

    public int Count => innerList.Count;

    public int Push(T item)
    {
        var p = innerList.Count;
        innerList.Add(item); // E[p] = O

        do
        {
            if (p == 0)
            {
                break;
            }

            var p2 = (p - 1) / 2;

            if (OnCompare(p, p2) < 0)
            {
                SwitchElements(p, p2);
                p = p2;
            }
            else
            {
                break;
            }
        } while (true);

        return p;
    }

    public T Pop()
    {
        var result = innerList[0];
        var p = 0;

        innerList[0] = innerList[innerList.Count - 1];
        innerList.RemoveAt(innerList.Count - 1);

        do
        {
            var pn = p;
            var p1 = 2 * p + 1;
            var p2 = 2 * p + 2;

            if (innerList.Count > p1 && OnCompare(p, p1) > 0)
            {
                p = p1;
            }

            if (innerList.Count > p2 && OnCompare(p, p2) > 0)
            {
                p = p2;
            }

            if (p == pn)
            {
                break;
            }

            SwitchElements(p, pn);
        } while (true);

        return result;
    }

    private void Update(int i)
    {
        var p = i;
        int p2;

        do
        {
            if (p == 0)
            {
                break;
            }

            p2 = (p - 1) / 2;

            if (OnCompare(p, p2) < 0)
            {
                SwitchElements(p, p2);
                p = p2;
            }
            else
            {
                break;
            }
        } while (true);

        if (p < i)
        {
            return;
        }

        do
        {
            var pn = p;
            var p1 = 2 * p + 1;
            p2 = 2 * p + 2;

            if (innerList.Count > p1 && OnCompare(p, p1) > 0)
            {
                p = p1;
            }

            if (innerList.Count > p2 && OnCompare(p, p2) > 0)
            {
                p = p2;
            }

            if (p == pn)
            {
                break;
            }

            SwitchElements(p, pn);
        } while (true);
    }

    private void SwitchElements(int i, int j)
    {
        var h = innerList[i];
        innerList[i] = innerList[j];
        innerList[j] = h;
    }

    private int OnCompare(int i, int j)
    {
        return comparer.Compare(innerList[i], innerList[j]);
    }
}