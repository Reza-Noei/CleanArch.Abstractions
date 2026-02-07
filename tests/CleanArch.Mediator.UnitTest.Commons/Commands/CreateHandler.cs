using CleanArch.Mediator.Contracts;
using CleanArch.Mediator.UnitTest.Commons.Dto;

namespace CleanArch.Mediator.UnitTest.Commons.Commands;

public class CreateHandler : ICommandHandler<Create, User>
{
    public async Task<User> HandleAsync(Create command, CancellationToken cancellationToken = default)
    {
        return new User
        {
            Id = 1,
            FirstName = command.FirstName,
            LastName = command.LastName,
        };
    }
}
