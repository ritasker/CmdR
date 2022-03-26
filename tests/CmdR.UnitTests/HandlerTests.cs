using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CmdR.Extensions;
using CmdR.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CmdR.UnitTests;

public class Ping
{
    public string Message { get; set; }
}

public class Pong
{
    public string Message { get; set; }
}

public class PingHandler : RequestHandler<Pong>
{
    public override void Configure()
    {
        Get("ping");
    }

    public override Task<Pong> HandleAsync(CancellationToken ct)
    {
        return Task.FromResult(new Pong
        {
            Message = "Pong"
        });
    }
}

public class PingPongHandler : RequestHandler<Ping, Pong>
{
    public override void Configure()
    {
        Post("ping-pong");
    }

    public override Task<Pong> HandleAsync(Ping ping, CancellationToken ct)
    {
        return Task.FromResult(new Pong
        {
            Message = ping.Message + "-Pong"
        });
    }
}

public class HandlerTests
{
    private HttpClient client;
    private TestServer server;

    [SetUp]
    public async Task Setup()
    {
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddRouting();
                services.AddCmdR(typeof(HandlerTests));
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints => endpoints.UseCmdR());
            });

        server = new TestServer(builder);
        await server.Host.StartAsync();
        
        client = server.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        server.Dispose();
        client.Dispose();
    }

    [Test]
    public async Task NoBodyRequestHandler_ShouldReturnOK()
    {
        var response = await client.GetAsync("ping");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var pong = await response.Content.ReadFromJsonAsync<Pong>();

        Assert.NotNull(pong);
        Assert.AreEqual("Pong", pong.Message);
    }
    
    [Test]
    public async Task BodyRequestHandler_ShouldReturnOK()
    {
        var ping = new Ping
        {
            Message = "Ping"
        };
        
        var response = await client.PostAsJsonAsync("ping-pong", ping);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var pong = await response.Content.ReadFromJsonAsync<Pong>();

        Assert.NotNull(pong);
        Assert.AreEqual("Ping-Pong", pong.Message);
    }
}