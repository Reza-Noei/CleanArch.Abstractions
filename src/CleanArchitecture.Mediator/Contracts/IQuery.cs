namespace CleanArchitecture.Mediator.Contracts;

/// <summary>
/// Represents a read-only request that retrieves data from the system.
/// </summary>
/// <typeparam name="TResponse">
/// The type of data returned by the query handler.
/// </typeparam>
/// <remarks>
/// Queries are part of the read side of CQRS and must not modify application 
/// state. Their primary purpose is data retrieval, projection, or filtering, 
/// typically optimized for read performance.
///
/// Every query is handled by exactly one query handler and is expected to 
/// be side-effect free.
/// </remarks>
public interface IQuery<TResponse> : IMessage where TResponse : class
{

}
