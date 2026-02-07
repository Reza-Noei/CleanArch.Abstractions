using CleanArch.Mediator.Contracts;
using CleanArch.Mediator.UnitTest.Commons.Dto;

namespace CleanArch.Mediator.UnitTest.Commons.Commands;

public class Create : ICommand<User>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
