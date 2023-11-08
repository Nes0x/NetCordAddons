# Modal handler

You can create class which will be representation of modal, by using it operating with modals will be easier.

Example: 
```csharp
public class ExampleModal : BaseModal
{
    public override string CustomId => "example";
    public override string Title => "Example";

    [ModalProperty("Example", TextInputStyle.Short)]
    public string ExampleProperty { get; init; }
}

public class ExampleModalSlashCommand : ApplicationCommandModule<SlashCommandContext>
{
    [SlashCommand("example", "Send a modal.", DMPermission = false,
        DefaultGuildUserPermissions = Permissions.Administrator)]
    public async Task HandleAsync()
    {
        var exampleModal = new ExampleModal();
        exampleModal.AddParameterToId(29482949249249);
        await RespondAsync(InteractionCallback.Modal(exampleModal.ToModalProperties(exampleModal.GetType())));
    }
}

public class ExampleModalHandler : InteractionModule<ModalSubmitInteractionContext>
{
    [Interaction("example")]
    public async Task HandleAsync(ulong id)
    {
        var modal = new ExampleModal();
        modal.Load(Context.Components, modal);
    }
}
```


