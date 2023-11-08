namespace NetCordAddons.Services.Modal;

[AttributeUsage(AttributeTargets.Property)]
public class MinLengthAttribute : Attribute
{
    public MinLengthAttribute(int? min)
    {
        Min = min;
    }

    public int? Min { get; init; }
}