namespace CleanArchitecture.Mediator.Contracts;

/// <summary>
/// Defines a handler responsible for processing a specific <see cref="IQuery{TResponse}"/>.
/// </summary>
/// <typeparam name="TQuery">
/// The type of query this handler processes.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of response returned after executing the query.
/// </typeparam>
/// <remarks>
/// A query handler encapsulates the read-side logic of a CQRS architecture.
/// It retrieves data without modifying system state and should remain 
/// side-effect free.
///
/// Each query is expected to have exactly one corresponding handler.
/// Pipeline behaviors (such as logging, caching, validation, authorization,
/// or metrics) may wrap around the execution of this handler.
/// </remarks>
public interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : class
{
    /// <summary>
    /// Executes the query logic and returns the requested data.
    /// </summary>
    /// <param name="query">The query request containing read criteria.</param>
    /// <param name="cancellationToken">
    /// A token that allows the operation to be canceled.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing 
    /// the query result of type <typeparamref name="TResponse"/>.
    /// </returns>
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}