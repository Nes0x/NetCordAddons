namespace NetCordAddons.Services.Models;

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
    public MaxLengthAttribute(int? max)
    {
        Max = max;
    }

    public int? Max { get; init; }
}