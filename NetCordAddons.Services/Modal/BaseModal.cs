using System.Reflection;
using NetCord;
using NetCord.Rest;

namespace NetCordAddons.Services.Modal;

public abstract class BaseModal
{
    protected abstract string CustomId { get; set; }
    protected abstract string ModalTitle { get; }


    public BaseModal AddParameterToId(params object[] objects)
    {
        foreach (var obj in objects) CustomId += $":{obj}";
        return this;
    }

    public ModalProperties ToModalProperties(params TextInputProperties[] toChange)
    {
        var properties = GetPropertiesWithModalPropertyAttribute()
            .Select(property =>
            {
                var modifiedTextInputProperties = toChange.FirstOrDefault(textInputProperties =>
                    textInputProperties.CustomId == property.Name);
                if (modifiedTextInputProperties is not null) return modifiedTextInputProperties;


                var modalProperty = property.GetCustomAttribute<ModalPropertyAttribute>()!;
                var label = string.IsNullOrWhiteSpace(modalProperty.Label) ? property.Name : modalProperty.Label;
                return new TextInputProperties(property.Name, modalProperty.Style, label)
                {
                    Value = modalProperty.Value,
                    Placeholder = modalProperty.Placeholder,
                    Required = modalProperty._required,
                    MinLength = modalProperty._minLength,
                    MaxLength = modalProperty._maxLength
                };
            });

        var modalProperties = new ModalProperties(CustomId, ModalTitle, properties);
        return modalProperties;
    }

    public void Load(IEnumerable<TextInput> textInputs)
    {
        var textInputsAsArray = textInputs.ToArray();
        var properties = GetPropertiesWithModalPropertyAttribute().Where(property =>
        {
            var textInput = textInputsAsArray.FirstOrDefault(textInput => textInput.CustomId == property.Name);
            return textInput is not null && property.Name == textInput.CustomId;
        });
        foreach (var property in properties)
            property.SetValue(this, textInputsAsArray.First(textInput => textInput.CustomId == property.Name).Value);
    }

    private IEnumerable<PropertyInfo> GetPropertiesWithModalPropertyAttribute()
    {
        return GetType().GetProperties()
            .Where(property => property.GetCustomAttributes(typeof(ModalPropertyAttribute), false).Length > 0);
    }
}