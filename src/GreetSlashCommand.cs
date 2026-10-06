using Ecs.Client;
using Greet.Data;
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Hosting.Services.ApplicationCommands;

namespace Greet.Adapter.NetCord;

public static class GreetSlashCommand
{
    public static void AddGreet(this IHost host, IWorldClient world) =>
        host.AddSlashCommand("greet", "Greet someone!",
            (User user, string message) => HandleAsync(world, user, message));

    public static async Task<string> HandleAsync(IWorldClient world, User user, string message)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var request = new GreetRequest { Message = message, Name = user.ToString() };
        var greeting = await world.AskAsync<GreetRequest, GreetResponse>(request, timeout.Token);

        return greeting.Text;
    }
}
