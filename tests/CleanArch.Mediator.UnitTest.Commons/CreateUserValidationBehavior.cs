using CleanArch.Mediator.Contracts;
using CleanArch.Mediator.UnitTest.Commons.Commands;
using CleanArch.Mediator.UnitTest.Commons.Dto;

namespace CleanArch.Mediator.UnitTest.Commons;

public class CreateUserValidationBehavior : IPipelineBehavior<Create, User>
{
    public Task<User> HandleAsync(Create request, PipelineHandlerDelegate<User> next, CancellationToken cancellationToken)
    {
        if (request.FirstName == null)
        {
            throw new ArgumentNullException(nameof(request.FirstName));
        }

        if (request.LastName == null)
        {
            throw new ArgumentNullException(nameof(request.LastName));
        }

        return next();
    }
}
