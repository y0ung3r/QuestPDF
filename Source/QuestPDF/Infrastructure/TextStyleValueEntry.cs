using System.Collections.Generic;

namespace QuestPDF.Infrastructure;

internal sealed class TextStyleValueEntry<T> : ITextStyleValueEntry
{
    public T? Value { get; }

    /// <inheritdoc />
    public bool HasValue
        => Value is not null;
        
    public TextStyleValueEntry(T? value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public bool Equals(ITextStyleValueEntry obj)
    {
        return obj is TextStyleValueEntry<T> other && EqualityComparer<T?>.Default.Equals(Value, other.Value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ITextStyleValueEntry other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return EqualityComparer<T?>.Default.GetHashCode(Value);
    }
}