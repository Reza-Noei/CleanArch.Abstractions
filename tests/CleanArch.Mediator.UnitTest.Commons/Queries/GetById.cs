using CleanArch.Mediator.Contracts;
using CleanArch.Mediator.UnitTest.Commons.Dto;

namespace CleanArch.Mediator.UnitTest.Commons.Queries;

public class GetById : IQuery<User>
{
    public int Id { get; set; }
}
