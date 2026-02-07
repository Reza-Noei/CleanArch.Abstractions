using CleanArch.Outbox.RabbitMq.HostedService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArch.Outbox.RabbitMq.Extensions;

public static class OutboxExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddMessageOutbox<TContext>() where TContext: OutboxContext
        { 
            // TODO: Register all required services for outbox pattern support.

            builder.Services.AddHostedService<OutboxHostedService>();

            builder.Services.AddHostedService<InboxHostedService>();

            return builder;
        }
    }
}
