using Ecs.Client;
using Greet.Data;
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Hosting.Services.ApplicationCommands;

namespace Greet.Adapter.NetCord;

public static class GreetSlashCommand
{
    // The lambda's parameters become the Discord options: user, message
    public static void AddGreet(this IHost host, IWorldClient world) =>
        host.AddSlashCommand("greet", "Greet someone!",
            (User user, string message) => HandleAsync(world, user, message));

    public static async Task<string> HandleAsync(IWorldClient world, User user, string message)
    {
        // Discord drops an interaction that isn't answered within 3 s
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        // Translate Discord into plain data: the world gets text, never a User
        var request = new GreetRequest { Message = message, Name = user.ToString() };
        var greeting = await world.AskAsync<GreetRequest, GreetResponse>(request, timeout.Token);

        return greeting.Text;
    }
}
