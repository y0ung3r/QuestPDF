using System.Collections.Generic;

namespace QuestPDF.Infrastructure;

internal abstract class TextStyleProperty
{
    internal static readonly TextStyleProperty<string?> Color = new(0);
        
    internal static readonly TextStyleProperty<string?> BackgroundColor = new(1);
        
    internal static readonly TextStyleProperty<string?> FontFamily = new(2);
        
    internal static readonly TextStyleProperty<float?> Size = new(3);
        
    internal static readonly TextStyleProperty<float?> LineHeight = new(4);
        
    internal static readonly TextStyleProperty<float?> LetterSpacing = new(5);
        
    internal static readonly TextStyleProperty<FontWeight?> FontWeight = new(6);
        
    internal static readonly TextStyleProperty<FontPosition?> FontPosition = new(7);
        
    internal static readonly TextStyleProperty<bool?> IsItalic = new(8);
        
    internal static readonly TextStyleProperty<bool?> HasStrikethrough = new(9);
        
    internal static readonly TextStyleProperty<bool?> HasUnderline = new(10);
        
    internal static readonly TextStyleProperty<bool?> WrapAnywhere = new(11);
        
    internal static readonly TextStyleProperty<TextStyle?> Fallback = new(12);
        
    internal static readonly TextStyleProperty<TextDirection?> Direction = new(13);

    internal static IEnumerable<TextStyleProperty> GetAll()
    {
        yield return Color;
        yield return BackgroundColor;
        yield return FontFamily;
        yield return Size;
        yield return LineHeight;
        yield return LetterSpacing;
        yield return FontWeight;
        yield return FontPosition;
        yield return IsItalic;
        yield return HasStrikethrough;
        yield return HasUnderline;
        yield return WrapAnywhere;
        yield return Fallback;
        yield return Direction;
    }
        
    public int Code { get; }

    protected TextStyleProperty(int code)
    {
        Code = code;
    }
}

internal sealed class TextStyleProperty<TValue> : TextStyleProperty
{
    public TextStyleProperty(int code)
        : base(code)
    { }
}