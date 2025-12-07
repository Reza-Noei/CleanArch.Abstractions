namespace CleanArchitecture.Mediator.Contracts;

/// <summary>
/// Defines a handler responsible for executing a state-changing command 
/// that does not produce a return value.
/// </summary>
/// <typeparam name="TCommand">
/// The type of command this handler processes.
/// </typeparam>
/// <remarks>
/// A command handler encapsulates the write-side logic of a CQRS architecture.
/// It modifies application state and should contain all logic required to perform 
/// the requested operation.
///
/// This form of command handler is appropriate when the caller only needs to know 
/// whether the operation completed successfully. Errors should be communicated 
/// through exceptions or middleware.
/// </remarks>
public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Executes the command and applies state changes to the system.
    /// </summary>
    /// <param name="command">The command request describing the desired action.</param>
    /// <param name="cancellationToken">A token to observe while waiting for completion.</param>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a handler responsible for executing a state-changing command 
/// that produces a response.
/// </summary>
/// <typeparam name="TCommand">
/// The type of command this handler processes.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of value returned after executing the command.
/// </typeparam>
/// <remarks>
/// Use this form when the caller needs a result—for example, the identifier of 
/// a newly created entity, a status object, or any domain-specific output.
///
/// A command handler should contain all logic related to performing the action, 
/// and pipeline behaviors may wrap the execution (validation, logging, 
/// transactions, authorization, etc.).
/// </remarks>
public interface ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : class
{
    /// <summary>
    /// Executes the command logic and returns the resulting response.
    /// </summary>
    /// <param name="command">The command request describing the desired action.</param>
    /// <param name="cancellationToken">A token to observe while waiting for completion.</param>
    /// <returns>
    /// A task representing the asynchronous operation containing the 
    /// response produced by the command execution.
    /// </returns>
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}