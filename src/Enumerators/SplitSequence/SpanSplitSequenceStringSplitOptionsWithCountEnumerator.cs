﻿namespace SpanExtensions;

/// <summary> 
/// Supports iteration over a <see cref="ReadOnlySpan{Char}"/> by splitting it at a specified delimiter and based on specified <see cref="StringSplitOptions"/>  with an upper limit of splits performed.  
/// </summary>   
public ref struct SpanSplitSequenceStringSplitOptionsWithCountEnumerator
{
    ReadOnlySpan<char> Span;
    readonly ReadOnlySpan<char> Delimiter;
    readonly StringSplitOptions Options;
    readonly int Count;
    int currentCount;

    public ReadOnlySpan<char> Current { get; internal set; }

    public SpanSplitSequenceStringSplitOptionsWithCountEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> delimiter, StringSplitOptions options, int count)
    {
        Span = span;
        Delimiter = delimiter;
        Options = options;
        Count = count;
    }

    public SpanSplitSequenceStringSplitOptionsWithCountEnumerator GetEnumerator()
    {
        return this;
    }

    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns><code>true</code> if the enumerator was successfully advanced to the next element; <code>false</code> if the enumerator has passed the end of the collection.</returns>
    public bool MoveNext()
    {
        ReadOnlySpan<char> span = Span;
        if (span.IsEmpty)
        {
            return false;
        }
        if (currentCount == Count)
        {
            return false;
        }
        int index = span.IndexOf(Delimiter);

        if (index == -1 || index >= span.Length)
        {
            Span = ReadOnlySpan<char>.Empty;
            Current = span;
            return true;
        }
        currentCount++;
        Current = span[..index];

        if (Options.HasFlag(StringSplitOptions.TrimEntries))
        {
            Current = Current.Trim();
        }
        if (Options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
        {
            if (Current.IsEmpty)
            {
                Span = span[(index + Delimiter.Length)..];
                return MoveNext();
            }
        }
        Span = span[(index + Delimiter.Length)..];
        return true;
    }

}
