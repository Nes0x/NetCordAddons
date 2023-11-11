using NetCord.Rest;
using NetCordAddons.Services.Modal;

namespace NetCordAddons.Examples.Modals;

public class ExampleModal : BaseModal
{
    protected override string CustomId { get; set; } = "example";
    protected override string EmbedTitle => "Example";

    [ModalProperty(TextInputStyle.Short, Placeholder = "a", Required = true)]
    public string ExampleProperty { get; init; }
}