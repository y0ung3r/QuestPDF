using System.Collections.Generic;

namespace QuestPDF.Infrastructure;

internal sealed class TextStyleDefaultValueEntry : ITextStyleValueEntry
{
    internal static readonly TextStyleDefaultValueEntry Instance = new();
    
    /// <inheritdoc />
    public bool HasValue
        => false;

    public bool Equals(ITextStyleValueEntry other)
    {
        return other is TextStyleDefaultValueEntry;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ITextStyleValueEntry other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return EqualityComparer<TextStyleDefaultValueEntry?>.Default.GetHashCode(this);
    }
    
    private TextStyleDefaultValueEntry()
    { }
}