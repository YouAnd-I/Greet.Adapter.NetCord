using Ecs.Client;
using Greet.Data;
using NetCord;
using NetCord.JsonModels;
using NetCord.Rest;
using Xunit;

namespace Greet.Adapter.NetCord.Tests;

public class GreetSlashCommandTests
{
    // Stands in for the whole world: adapter tests need no ECS and no Discord
    private sealed class StubWorld(object response) : IWorldClient
    {
        public object? Request { get; private set; }

        public Task<TResponse> AskAsync<TRequest, TResponse>(
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            Request = request;
            return Task.FromResult((TResponse)response);
        }
    }

    [Fact]
    public async Task Greet_SendsPlainData_AndRepliesWithTheWorldsText()
    {
        var world = new StubWorld(new GreetResponse { Text = "hi, <@42>!" });
        using var rest = new RestClient();
        var alice = new User(new JsonUser { Id = 42, Username = "alice" }, rest);

        var reply = await GreetSlashCommand.HandleAsync(world, alice, "hi");

        var request = Assert.IsType<GreetRequest>(world.Request);
        Assert.Equal("hi", request.Message);
        Assert.Equal("<@42>", request.Name);
        Assert.Equal("hi, <@42>!", reply);
    }
}
