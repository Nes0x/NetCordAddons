namespace NetCordAddons.Services.Validators;

internal interface IServiceValidator
{
    bool ShouldBeAddedAsModules(Type serviceType, bool areCommands);
}