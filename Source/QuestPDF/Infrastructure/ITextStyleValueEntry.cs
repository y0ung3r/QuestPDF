using System;

namespace QuestPDF.Infrastructure;

internal interface ITextStyleValueEntry : IEquatable<ITextStyleValueEntry>
{
    bool HasValue { get; }
}