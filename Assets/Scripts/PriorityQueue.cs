using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<T> elements = new List<T>();
    private System.Func<T, int> priorityFunction;

    public PriorityQueue(System.Func<T, int> priorityFunction)
    {
        this.priorityFunction = priorityFunction;
    }

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item)
    {
        elements.Add(item);
        elements.Sort((a, b) => priorityFunction(a).CompareTo(priorityFunction(b)));
    }

    public T Dequeue()
    {
        if (elements.Count > 0)
        {
            T item = elements[0];
            elements.RemoveAt(0);
            return item;
        }
        return default(T);
    }

    public bool Contains(T item)
    {
        return elements.Contains(item);
    }
}
