using NetCord.Services.Commands;

namespace NetCordAddons.Services.Models;

public class CommandSettings<TContext> where TContext : ICommandContext
{
    public CommandSettings(string prefix, CommandServiceConfiguration<TContext>? commandServiceConfiguration = null)
    {
        CommandServiceConfiguration = commandServiceConfiguration;
        Prefix = prefix;
    }

    public string Prefix { get; }
    public CommandServiceConfiguration<TContext>? CommandServiceConfiguration { get; }
}