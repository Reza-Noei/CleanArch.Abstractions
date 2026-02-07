using MediatR;
namespace CleanArch.Outbox;

public interface IMessageHandler : IRequestHandler<IMessage>
{

}