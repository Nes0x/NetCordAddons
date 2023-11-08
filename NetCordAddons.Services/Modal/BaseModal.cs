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

    public ModalProperties ToModalProperties(Type createdModalType)
    {
        var properties = GetPropertiesWithModalPropertyAttribute(createdModalType)
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
        Console.WriteLine(modalProperties.CustomId);
        return modalProperties;
    }

    public void Load(IEnumerable<TextInput> textInputs, object customModal)
    {
        var type = customModal.GetType();
        var properties = GetPropertiesWithModalPropertyAttribute(type);
        foreach (var textInput in textInputs)
        {
            var property = properties.FirstOrDefault(property => property.Name == textInput.CustomId);
            if (string.IsNullOrEmpty(textInput.Value) || property is null) continue;
            property.SetValue(customModal, textInput.Value);
        }
    }

    private IEnumerable<PropertyInfo> GetPropertiesWithModalPropertyAttribute(Type type)
    {
        return type.GetProperties()
            .Where(m => m.GetCustomAttributes(typeof(ModalPropertyAttribute), false).Length > 0);
    }
}