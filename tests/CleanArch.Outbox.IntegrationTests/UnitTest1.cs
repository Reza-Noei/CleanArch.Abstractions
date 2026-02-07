using Aspire.Hosting.Testing;
using Projects;

namespace CleanArch.Outbox.IntegrationTests;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<OutboxOrchestrator>();

        await using var app = await builder.BuildAsync();

        await app.StartAsync();

        var httpClient = app.CreateHttpClient("webfrontend");
    }
}
