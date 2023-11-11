using NetCord.Rest;

namespace NetCordAddons.Services.Modal;

[AttributeUsage(AttributeTargets.Property)]
public class ModalPropertyAttribute : Attribute
{
    internal int? _maxLength;
    internal int? _minLength;

    internal bool? _required;

    public ModalPropertyAttribute(TextInputStyle style)
    {
        Style = style;
    }

    public string? Label { get; init; }
    public string? Value { get; init; }
    public string? Placeholder { get; init; }
    public TextInputStyle Style { get; init; }

    public bool Required
    {
        get => _required.GetValueOrDefault();
        init => _required = value;
    }

    public int MaxLength
    {
        get => _maxLength.GetValueOrDefault();
        init => _maxLength = value;
    }

    public int MinLength
    {
        get => _minLength.GetValueOrDefault();
        init => _minLength = value;
    }
}