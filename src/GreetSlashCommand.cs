using Discord.Greet.Data;
using Discord.Greet.System.Frent;
using Frent;
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Hosting.Services.ApplicationCommands;

namespace Discord.Greet.System.NetCord;

public static class GreetSlashCommand
{
    public static void AddGreet(this IHost host, World world)
    {
        host.AddSlashCommand("greet", "Greet someone!", (User user, string message) =>
        {
            var request = world.Create(
                new GreetRequestTag(),
                new GreetInput { UserId = user.Id, Display = user.ToString(), Message = message });
            GreetSystem.Execute(world);
            var text = request.Get<GreetResponse>().Text;
            request.Delete();
            return text;
        });
    }
}
