namespace NetCordAddons.Services.Models;

[AttributeUsage(AttributeTargets.Property)]
public class MinLengthAttribute : Attribute
{
    public MinLengthAttribute(int? min)
    {
        Min = min;
    }

    public int? Min { get; init; }
}