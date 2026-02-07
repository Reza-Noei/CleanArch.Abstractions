using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Outbox.Extensions;

internal class InboxMessageOnlySupportOption : IDbContextOptionsExtension
{
    public DbContextOptionsExtensionInfo Info { get; }

    public void ApplyServices(IServiceCollection services)
    {

    }

    public void Validate(IDbContextOptions options)
    {

    }
}