using NetCord.Rest;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Examples.Modals;

public class ExampleModal : BaseModal
{
    protected override string CustomId { get; set; } = "example";
    protected override string Title => "Example";

    [ModalProperty("Example", TextInputStyle.Short, Placeholder = "a")]
    [Required]
    public string ExampleProperty { get; init; }
}