using System.Runtime.CompilerServices;

namespace CleanArchitecture.Mediator.Contracts;

/// <summary>
/// Represents a command that performs an action but does not return any meaningful result.
/// </summary>
/// <remarks>
/// This interface is a specialization of <see cref="ICommand{TResponse}"/> where the
/// response type is <see cref="Unit"/>. It is intended for commands that are
/// executed for their side effects only, such as creating, updating, or deleting
/// entities, without returning a value.
///
/// Using <see cref="Unit"/> as the response allows pipeline behaviors and mediator
/// dispatching to remain fully generic and type-safe, even for commands that produce
/// no response.
/// </remarks>
public interface ICommand : ICommand<Unit>
{

}

/// <summary>
/// Represents a state-changing operation that produces a response.
/// </summary>
/// <typeparam name="TResponse">
/// The type of value returned by the command handler.
/// </typeparam>
/// <remarks>
/// This generic command form is used when the caller expects a result—such as 
/// a created entity ID, a status object, or any domain-specific output.
///
/// Like all commands, this message is processed by a single command handler 
/// and participates in all mediator pipeline behaviors (logging, validation, etc.).
/// </remarks>
public interface ICommand<TResponse> : IMessage where TResponse : class
{

}
