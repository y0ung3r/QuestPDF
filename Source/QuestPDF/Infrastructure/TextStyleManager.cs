using System.Collections.Concurrent;
using System.Linq;

namespace QuestPDF.Infrastructure
{
    internal record StylesApplyingOptions
    {
        public bool OverrideStyle { get; set; }
            
        public bool OverrideFontFamily { get; set; }
            
        public bool AllowFallback { get; set; }
    }
    
    internal static class TextStyleManager
    {
        private static readonly ConcurrentDictionary<(TextStyle Origin, TextStyleProperty Property, ITextStyleValueEntry Accessor), TextStyle> TextStyleMutateCache = new();
        private static readonly ConcurrentDictionary<(TextStyle Origin, TextStyle Parent), TextStyle> TextStyleApplyInheritedCache = new();
        private static readonly ConcurrentDictionary<TextStyle, TextStyle> TextStyleApplyGlobalCache = new();
        private static readonly ConcurrentDictionary<(TextStyle Origin, TextStyle Parent), TextStyle> TextStyleOverrideCache = new();

        public static TextStyle Mutate<TValue>(this TextStyle origin, TextStyleProperty<TValue> property, TValue value)
        {
            var cacheKey = (origin, property, new TextStyleValueEntry<TValue>(value));
            return TextStyleMutateCache.GetOrAdd(cacheKey, tuple => tuple.Origin.MutateStyle(tuple.Property, tuple.Accessor, overrideValue: true));
        }

        private static TextStyle MutateStyle(this TextStyle origin, TextStyleProperty property, ITextStyleValueEntry newValueEntry, bool overrideValue)
        {
            if (overrideValue && !newValueEntry.HasValue)
                return origin;

            var oldValueEntry = origin.GetValueEntry(property);

            if (!overrideValue && oldValueEntry.HasValue)
                return origin;

            if (oldValueEntry.Equals(newValueEntry))
                return origin;

            var textStyleClone = origin with { };
            textStyleClone.ReplaceEntry(property, newValueEntry);
            
            return textStyleClone;
        }

        internal static TextStyle ApplyInheritedStyle(this TextStyle style, TextStyle parent)
        {
            var cacheKey = (style, parent);
            var options = new StylesApplyingOptions
            {
                OverrideStyle = false,
                OverrideFontFamily = false,
                AllowFallback = true
            };
            
            return TextStyleApplyInheritedCache.GetOrAdd(cacheKey, tuple => tuple.Origin.ApplyStyleProperties(tuple.Parent, options).UpdateFontFallback(overrideStyle: true));
        }
        
        internal static TextStyle ApplyGlobalStyle(this TextStyle style)
        {
            var options = new StylesApplyingOptions
            {
                OverrideStyle = false,
                OverrideFontFamily = false,
                AllowFallback = true
            };
            
            return TextStyleApplyGlobalCache.GetOrAdd(style, tuple => tuple.ApplyStyleProperties(TextStyle.LibraryDefault, options).UpdateFontFallback(overrideStyle: false));
        }
        
        private static TextStyle UpdateFontFallback(this TextStyle style, bool overrideStyle)
        {
            var options = new StylesApplyingOptions
            {
                OverrideStyle = overrideStyle,
                OverrideFontFamily = false,
                AllowFallback = false
            };
            
            var targetFallbackStyle = style
                .Fallback?
                .ApplyStyleProperties(style, options)
                .UpdateFontFallback(overrideStyle);
            
            return style.MutateStyle(TextStyleProperty.Fallback, new TextStyleValueEntry<TextStyle>(targetFallbackStyle), overrideValue: true);
        }
        
        internal static TextStyle OverrideStyle(this TextStyle style, TextStyle parent)
        {
            var cacheKey = (style, parent);
            var options = new StylesApplyingOptions
            {
                OverrideStyle = true,
                OverrideFontFamily = true,
                AllowFallback = true
            };
            
            return TextStyleOverrideCache.GetOrAdd(cacheKey, tuple => tuple.Origin.ApplyStyleProperties(tuple.Parent, options));
        }

        private static TextStyle ApplyStyleProperties(this TextStyle style, TextStyle parentStyle, StylesApplyingOptions options)
        {
            return TextStyleProperty
                .GetAll()
                .Aggregate(style, (mutableStyle, nextProperty) =>
                {
                    if (!CanBeMutate(nextProperty, mutableStyle, options))
                        return mutableStyle;

                    return mutableStyle.MutateStyle(
                        nextProperty,
                        parentStyle.GetValueEntry(nextProperty),
                        overrideValue: options.OverrideStyle);
                });

            static bool CanBeMutate(TextStyleProperty property, TextStyle mutableStyle, StylesApplyingOptions applyingOptions)
            {
                if (property == TextStyleProperty.FontFamily)
                    return string.IsNullOrWhiteSpace(mutableStyle.FontFamily) || applyingOptions.OverrideFontFamily;
                
                if (property == TextStyleProperty.Fallback)
                    return applyingOptions.AllowFallback;
                
                return true;
            }
        }
    }
}