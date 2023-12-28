using System.Collections.Concurrent;
using System.Linq;
using QuestPDF.Helpers;

namespace QuestPDF.Infrastructure
{
    public record TextStyle
    {
        private readonly ConcurrentDictionary<int, ITextStyleValueEntry> _propertyEntries
            = new(TextStyleProperty
                .GetAll()
                .ToDictionary<TextStyleProperty, int, ITextStyleValueEntry>(
                    property => property.Code, 
                    _ => TextStyleDefaultValueEntry.Instance));

        protected TextStyle(TextStyle clone)
        {
            _propertyEntries = new ConcurrentDictionary<int, ITextStyleValueEntry>(clone._propertyEntries);
        }

        internal string? Color
        {
            get => GetValue(TextStyleProperty.Color);
            set => SetValue(TextStyleProperty.Color, value);
        }

        internal string? BackgroundColor
        {
            get => GetValue(TextStyleProperty.BackgroundColor);
            set => SetValue(TextStyleProperty.BackgroundColor, value);
        }
        
        internal string? FontFamily 
        {
            get => GetValue(TextStyleProperty.FontFamily);
            set => SetValue(TextStyleProperty.FontFamily, value);
        }
        
        internal float? Size
        {
            get => GetValue(TextStyleProperty.Size);
            set => SetValue(TextStyleProperty.Size, value);
        }
        
        internal float? LineHeight
        {
            get => GetValue(TextStyleProperty.LineHeight);
            set => SetValue(TextStyleProperty.LineHeight, value);
        }
        
        internal float? LetterSpacing
        {
            get => GetValue(TextStyleProperty.LetterSpacing);
            set => SetValue(TextStyleProperty.LetterSpacing, value);
        }

        internal FontWeight? FontWeight
        {
            get => GetValue(TextStyleProperty.FontWeight);
            set => SetValue(TextStyleProperty.FontWeight, value);
        }

        internal FontPosition? FontPosition
        {
            get => GetValue(TextStyleProperty.FontPosition);
            set => SetValue(TextStyleProperty.FontPosition, value);
        }

        internal bool? IsItalic
        {
            get => GetValue(TextStyleProperty.IsItalic);
            set => SetValue(TextStyleProperty.IsItalic, value);
        }

        internal bool? HasStrikethrough
        {
            get => GetValue(TextStyleProperty.HasStrikethrough);
            set => SetValue(TextStyleProperty.HasStrikethrough, value);
        }

        internal bool? HasUnderline
        {
            get => GetValue(TextStyleProperty.HasUnderline);
            set => SetValue(TextStyleProperty.HasUnderline, value);
        }

        internal bool? WrapAnywhere
        {
            get => GetValue(TextStyleProperty.WrapAnywhere);
            set => SetValue(TextStyleProperty.WrapAnywhere, value);
        }

        internal TextDirection? Direction
        {
            get => GetValue(TextStyleProperty.Direction);
            set => SetValue(TextStyleProperty.Direction, value);
        }

        internal TextStyle? Fallback
        {
            get => GetValue(TextStyleProperty.Fallback);
            set => SetValue(TextStyleProperty.Fallback, value);
        }
        
        internal static TextStyle LibraryDefault { get; } = new()
        {
            Color = Colors.Black,
            BackgroundColor = Colors.Transparent,
            FontFamily = Fonts.Lato,
            Size = 12,
            LineHeight = 1.2f,
            LetterSpacing = 0,
            FontWeight = Infrastructure.FontWeight.Normal,
            FontPosition = Infrastructure.FontPosition.Normal,
            IsItalic = false,
            HasStrikethrough = false,
            HasUnderline = false,
            WrapAnywhere = false,
            Direction = TextDirection.Auto,
            Fallback = null
        };

        public static TextStyle Default { get; } = new();

        internal ITextStyleValueEntry GetValueEntry(TextStyleProperty property)
        {
            return _propertyEntries.GetOrAdd(
                property.Code, 
                _ => TextStyleDefaultValueEntry.Instance);
        }

        internal void ReplaceEntry(TextStyleProperty property, ITextStyleValueEntry entry)
        {
            _propertyEntries.AddOrUpdate(
                property.Code,
                _ => TextStyleDefaultValueEntry.Instance,
                (_, _) => entry);
        }

        private TValue? GetValue<TValue>(TextStyleProperty<TValue> property)
        {
            if (GetValueEntry(property) is TextStyleValueEntry<TValue> entry) 
                return entry.Value;

            return default(TValue?);
        }

        private void SetValue<TValue>(TextStyleProperty<TValue> property, TValue? value)
        {
            ReplaceEntry(property, new TextStyleValueEntry<TValue>(value));
        }
    }
}