﻿namespace SpanExtensions;

/// <summary>
/// Supports iteration over a <see cref="ReadOnlySpan{T}"/> by splitting it at specified delimiters of type <typeparamref name="T"/>. 
/// </summary>
/// <typeparam name="T">The type of elements in the enumerated <see cref="ReadOnlySpan{T}"/></typeparam>  
public ref struct SpanSplitAnyEnumerator<T> where T : IEquatable<T>
{
    ReadOnlySpan<T> Span;
    readonly ReadOnlySpan<T> Delimiters;

    public ReadOnlySpan<T> Current { get; internal set; }

    public SpanSplitAnyEnumerator(ReadOnlySpan<T> span, ReadOnlySpan<T> delimiters)
    {
        Span = span;
        Delimiters = delimiters;
    }

    public SpanSplitAnyEnumerator<T> GetEnumerator()
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
        int index = span.IndexOfAny(Delimiters);

        if (index == -1 || index >= span.Length)
        {
            Span = ReadOnlySpan<T>.Empty;
            Current = span;
            return true;
        }
        Current = span[..index];
        Span = span[(index + 1)..];
        return true;
    }

}
