using CleanArch.Mediator.Contracts;

namespace CleanArch.Mediator.UnitTest.Commons.Commands;

public class Update : ICommand
{
    public int Id { get; set; }
}
