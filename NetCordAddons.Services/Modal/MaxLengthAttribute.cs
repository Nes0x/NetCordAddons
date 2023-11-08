namespace NetCordAddons.Services.Models;

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
    public int? Max { get; init; }

    public MaxLengthAttribute(int? max)
    {
        Max = max;
    }
}