using CleanArch.Mediator.Contracts;

namespace CleanArch.Mediator.UnitTest.Commons.Commands;

public class UpdateHandler : ICommandHandler<Update>
{
    public async Task HandleAsync(Update command, CancellationToken cancellationToken = default)
    {

    }
}