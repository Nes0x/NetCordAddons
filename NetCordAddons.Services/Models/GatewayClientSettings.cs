﻿using NetCord;
using NetCord.Gateway;

namespace NetCordAddons.Services.Models;

public class GatewayClientSettings : IClientSettings
{
    public GatewayClientSettings(Token token, GatewayClientConfiguration? gatewayClientConfiguration = null)
    {
        Token = token;
        GatewayClientConfiguration = gatewayClientConfiguration;
    }

    public GatewayClientConfiguration? GatewayClientConfiguration { get; }

    public Token Token { get; }
}