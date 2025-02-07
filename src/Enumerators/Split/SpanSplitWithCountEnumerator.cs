﻿namespace SpanExtensions;

/// <summary>
/// Supports iteration over a <see cref="ReadOnlySpan{T}"/> by splitting it a a specified delimiter of type <typeparamref name="T"/> with an upper limit of splits performed.
/// </summary>
/// <typeparam name="T">The type of elements in the enumerated <see cref="ReadOnlySpan{T}"/></typeparam>
public ref struct SpanSplitWithCountEnumerator<T> where T : IEquatable<T>
{
    ReadOnlySpan<T> Span;
    readonly T Delimiter;
    readonly int Count;
    int currentCount;

    public ReadOnlySpan<T> Current { get; internal set; }

    public SpanSplitWithCountEnumerator(ReadOnlySpan<T> span, T delimiter, int count)
    {
        Span = span;
        Delimiter = delimiter;
        Count = count;
    }

    public SpanSplitWithCountEnumerator<T> GetEnumerator()
    {
        return this;
    }

    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns><code>true</code> if the enumerator was successfully advanced to the next element; <code>false</code> if the enumerator has passed the end of the collection.</returns>
    public bool MoveNext()
    {
        ReadOnlySpan<T> span = Span;
        if (span.IsEmpty)
        {
            return false;
        }
        if(currentCount == Count)
        { 
            return false;
        }
        int index = span.IndexOf(Delimiter);
        if (index == -1 || index >= span.Length)
        {
            Span = ReadOnlySpan<T>.Empty;
            Current = span;
            return true;
        }
        currentCount++;
        Current = span[..index];
        Span = span[(index + 1)..];
        return true;
    }

}
