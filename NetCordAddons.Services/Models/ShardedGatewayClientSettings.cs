using NetCord;
using NetCord.Gateway;

namespace NetCordAddons.Services.Models;

public class ShardedGatewayClientSettings : IClientSettings
{
    public ShardedGatewayClientSettings(Token token,
        ShardedGatewayClientConfiguration? shardedGatewayClientConfiguration = null)
    {
        Token = token;
        ShardedGatewayClientConfiguration = shardedGatewayClientConfiguration;
    }

    public ShardedGatewayClientConfiguration? ShardedGatewayClientConfiguration { get; }

    public Token Token { get; }
}