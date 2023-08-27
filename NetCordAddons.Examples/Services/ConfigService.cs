using System.Text.Json;

namespace NetCordAddons.Examples.Services;

public class ConfigService
{
    public string Token { get; init; }

    public static ConfigService Create()
    {
        return JsonSerializer.Deserialize<ConfigService>(File.ReadAllText("appsettings.json"));
    }
}