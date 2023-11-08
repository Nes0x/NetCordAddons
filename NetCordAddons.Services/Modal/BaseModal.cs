using System.Reflection;
using NetCord;
using NetCord.Rest;

namespace NetCordAddons.Services.Models;

public abstract class BaseModal
{
    protected abstract string CustomId { get; set; }
    protected abstract string Title { get; }


    public void AddParameterToId(object obj)
    {
        CustomId += $":{obj}";
    }

    public ModalProperties ToModalProperties()
    {
        var properties = GetPropertiesWithModalPropertyAttribute()
            .Select(property =>
            {
                var modalProperty = property.GetCustomAttribute<ModalPropertyAttribute>()!;
                var maxLength = property.GetCustomAttribute<MaxLengthAttribute>();
                var minLength = property.GetCustomAttribute<MinLengthAttribute>();
                var required = property.GetCustomAttribute<RequiredAttribute>();
                return new TextInputProperties(property.Name, modalProperty.TextInputStyle, modalProperty.Label)
                {
                    Value = modalProperty.Value,
                    Placeholder = modalProperty.Placeholder,
                    Required = required is not null,
                    MinLength = minLength?.Min,
                    MaxLength = maxLength?.Max
                };
            });

        var modalProperties = new ModalProperties(CustomId, Title, properties);
        return modalProperties;
    }

    public void Load(IEnumerable<TextInput> textInputs)
    {
        var properties = GetPropertiesWithModalPropertyAttribute();
        foreach (var textInput in textInputs)
        {
            var property = properties.FirstOrDefault(property => property.Name == textInput.CustomId);
            if (string.IsNullOrEmpty(textInput.Value) || property is null) continue;
            property.SetValue(this, textInput.Value);
        }
    }

    private IEnumerable<PropertyInfo> GetPropertiesWithModalPropertyAttribute()
    {
        return GetType().GetProperties()
            .Where(m => m.GetCustomAttributes(typeof(ModalPropertyAttribute), false).Length > 0);
    }
}