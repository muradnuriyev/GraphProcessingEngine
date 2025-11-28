namespace GraphProcessingEngine.Core.PathFinding.PriorityQueue;

/// <summary>
/// Minimal binary heap priority queue; keeps smallest priority on top.
/// </summary>
internal sealed class BinaryHeapPriorityQueue<T>
{
    private readonly List<(T Item, double Priority, long Sequence)> _heap = new();
    private long _sequence;

    public int Count => _heap.Count;

    public void Enqueue(T item, double priority)
    {
        _heap.Add((item, priority, _sequence++));
        HeapifyUp(_heap.Count - 1);
    }

    public bool TryDequeue(out T? item, out double priority)
    {
        if (_heap.Count == 0)
        {
            item = default;
            priority = default;
            return false;
        }

        (item, priority, _) = _heap[0];
        var last = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        if (_heap.Count > 0)
        {
            _heap[0] = last;
            HeapifyDown(0);
        }

        return true;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            var parent = (index - 1) / 2;
            if (Compare(_heap[index], _heap[parent]) >= 0)
            {
                return;
            }

            (_heap[parent], _heap[index]) = (_heap[index], _heap[parent]);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            var left = 2 * index + 1;
            var right = 2 * index + 2;
            var smallest = index;

            if (left < _heap.Count && Compare(_heap[left], _heap[smallest]) < 0)
            {
                smallest = left;
            }

            if (right < _heap.Count && Compare(_heap[right], _heap[smallest]) < 0)
            {
                smallest = right;
            }

            if (smallest == index)
            {
                return;
            }

            (_heap[index], _heap[smallest]) = (_heap[smallest], _heap[index]);
            index = smallest;
        }
    }

    private static int Compare((T Item, double Priority, long Sequence) a, (T Item, double Priority, long Sequence) b)
    {
        var cmp = a.Priority.CompareTo(b.Priority);
        return cmp != 0 ? cmp : a.Sequence.CompareTo(b.Sequence);
    }
}
