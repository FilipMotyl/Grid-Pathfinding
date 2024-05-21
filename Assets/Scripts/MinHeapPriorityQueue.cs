using System.Collections.Generic;
using System;

public class MinHeapPriorityQueue<T> where T : IComparable<T>
{
    private List<T> elements = new List<T>();
    private Dictionary<T, int> elementIndices = new Dictionary<T, int>();

    public int Count => elements.Count;

    public void Enqueue(T item)
    {
        elements.Add(item);
        int index = elements.Count - 1;
        elementIndices[item] = index;
        HeapifyUp(index);
    }

    public T Dequeue()
    {
        if (elements.Count > 0)
        {
            T item = elements[0];
            T lastItem = elements[elements.Count - 1];
            elements[0] = lastItem;
            elementIndices[lastItem] = 0;
            elements.RemoveAt(elements.Count - 1);
            elementIndices.Remove(item);
            HeapifyDown(0);
            return item;
        }
        return default(T);
    }

    public bool Contains(T item)
    {
        return elementIndices.ContainsKey(item);
    }

    public void UpdateItem(T item)
    {
        if (elementIndices.TryGetValue(item, out int index))
        {
            HeapifyUp(index);
            HeapifyDown(index);
        }
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (elements[index].CompareTo(elements[parentIndex]) >= 0)
            {
                break;
            }
            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = elements.Count - 1;
        while (index < lastIndex)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int smallestIndex = index;

            if (leftChildIndex <= lastIndex && elements[leftChildIndex].CompareTo(elements[smallestIndex]) < 0)
            {
                smallestIndex = leftChildIndex;
            }

            if (rightChildIndex <= lastIndex && elements[rightChildIndex].CompareTo(elements[smallestIndex]) < 0)
            {
                smallestIndex = rightChildIndex;
            }

            if (smallestIndex == index)
            {
                break;
            }

            Swap(index, smallestIndex);
            index = smallestIndex;
        }
    }

    private void Swap(int indexA, int indexB)
    {
        T temp = elements[indexA];
        elements[indexA] = elements[indexB];
        elements[indexB] = temp;

        elementIndices[elements[indexA]] = indexA;
        elementIndices[elements[indexB]] = indexB;
    }
}