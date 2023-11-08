using NetCord.Rest;

namespace NetCordAddons.Services.Models;

[AttributeUsage(AttributeTargets.Property)]
public class ModalPropertyAttribute : Attribute
{
    public ModalPropertyAttribute(string label, TextInputStyle textInputStyle)
    {
        Label = label;
        TextInputStyle = textInputStyle;
    }

    public string Label { get; init; }
    public string Value { get; init; }
    public TextInputStyle TextInputStyle { get; init; }
    // public int? MinLength { get; init; }
    // public int? MaxLength { get; init; }
    // public bool? Required { get; init; }
    public string? Placeholder { get; init; }
}