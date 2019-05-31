using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A generic heap collection.
/// </summary>
/// <typeparam name="T">The type of object storerd in the heap</typeparam>
[System.Serializable]
public class Heap<T>
{
    #region Static Helper Methods

    /// <summary>
    /// Returns the index of the parent for the given index.
    /// </summary>
    /// <param name="i">The child index.</param>
    /// <returns>The parent index for the given child index.</returns>
    private static int Parent(int i)
    {
        return ((i + 1) / 2) - 1;
    }

    /// <summary>
    /// Returns the child index to the left of the given index.
    /// </summary>
    /// <param name="i">The parent index.</param>
    /// <returns>The left child index.</returns>
    private static int LeftChild(int i)
    {
        return i * 2 + 1;
    }

    /// <summary>
    /// Returns the child index to the right of the given index.
    /// </summary>
    /// <param name="i">The parent index.</param>
    /// <returns>The right child index.</returns>
    private static int RightChild(int i)
    {
        return i * 2 + 2;
    }

    #endregion

    // The elements stored within the heap.
    [SerializeField] protected List<T> elements = new List<T>();

    // The comparer used to comparer the elements within the heap.
    private IComparer<T> m_comparer = Comparer<T>.Default;

    #region Constructors

    /// <summary>
    /// Constructs a new empty heap.
    /// </summary>
    public Heap()
    {
        elements = new List<T>();
    }

    /// <summary>
    /// Constructs a new heap containing the given elements.
    /// </summary>
    /// <param name="elements">The elements to add to the heap.</param>
    public Heap(T[] elements) : this()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            Push(elements[i]);
        }
        
    }

    /// <summary>
    /// Constructs a new empty heap with a specified comparer.
    /// </summary>
    /// <param name="comparer">The comparer to be used by the heap.</param>
    public Heap(IComparer<T> comparer) : this()
    {
        this.comparer = comparer;
    }

    /// <summary>
    /// Constructs a new heap containing the given elements using the specified comparer. 
    /// </summary>
    /// <param name="elements">The elements to add to the heap.</param>
    /// <param name="comparer">The comparer to be used by the heap.</param>
    public Heap(T[] elements, IComparer<T> comparer) : this(comparer)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            Push(elements[i]);
        }
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Get and set the comparer to be used by the heap.
    /// </summary>
    public IComparer<T> comparer
    {
        get { return m_comparer; }
        set { m_comparer = value; }
    }

    /// <summary>
    /// Returns the number of elements in the heap.
    /// </summary>
    public int count
    {
        get { return elements.Count; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Removes all the elements from the heap.
    /// </summary>
    public void Clear()
    {
        elements.Clear();
    }

    /// <summary>
    /// Adds a new element to the heap.
    /// </summary>
    /// <param name="element">The element to be added.</param>
    public void Push(T element)
    {
        int i = elements.Count;
        elements.Add(element);
        int parent = Parent(i);
        while (i > 0 && Compare(i, parent) < 0)
        {
            Swap(i, parent);
            i = parent;
            parent = Parent(i);
        }
    }

    /// <summary>
    /// Returns the element at the top of the heap.
    /// Throw an out of range exception if the heap is empty.
    /// </summary>
    /// <returns>The element at the top of the heap.</returns>
    public T Peek()
    {
        if (count == 0)
            throw new ArgumentOutOfRangeException("Attempted to peek an empty heap");

        return elements[0];
    }

    /// <summary>
    /// Removes and returns the element at the top of the heap.
    /// Throw an out of range exception if the heap is empty.
    /// </summary>
    /// <returns>The removed element.</returns>
    public T Pop()
    {
        if (count == 0)
            throw new ArgumentOutOfRangeException("Attempted to pop an element from an empty heap");

        var value = elements[0];
        elements[0] = elements[count - 1];
        elements.RemoveAt(count - 1);
        Heapify(0);
        return value;
    }

    /// <summary>
    /// Removes the specified element from the heap.
    /// </summary>
    /// <param name="element">The element to remove.</param>
    /// <returns>True if the element has been removed, otherwise false.</returns>
    public bool Remove(T element)
    {
        if (elements.Remove(element))
        {
            Heapify(0);
            return true;
        }
        return false;
    }

    #endregion

    #region Private Methods

    private int Compare(int a, int b)
    {
        return comparer.Compare(elements[a], elements[b]);
    }

    private void Swap(int a, int b)
    {
        T temp = elements[a];
        elements[a] = elements[b];
        elements[b] = temp;
    }

    protected void Heapify(int i)
    {
        while (i < count)
        {
            int left = LeftChild(i);
            if (left >= count) break;

            int child = left;
            int right = RightChild(i);
            if (right < count && Compare(right, left) < 0)
                child = right;

            if (Compare(i, child) < 0) break;
            Swap(i, child);
            i = child;
        }
    }

    #endregion
}
