namespace CleanArchitecture.Mediator.Contracts;

/// <summary>
/// Represents the central dispatching point for application messages.
/// 
/// <para>
/// The mediator receives <see cref="ICommand"/> and <see cref="IQuery{TResponse}"/> 
/// objects and routes them to the appropriate handler. 
/// This keeps your application code decoupled from handler implementations 
/// and promotes a clean CQRS-style architecture.
/// </para>
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a command that does not produce a response.
    /// <para>
    /// Use this for "fire-and-forget" commands such as:
    /// <list type="bullet">
    /// <item><description>CreateOrderCommand</description></item>
    /// <item><description>DeleteUserCommand</description></item>
    /// <item><description>PublishEventCommand</description></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="command">The command instance to dispatch.</param>
    /// <param name="cancellationToken">A token for cancelling the operation.</param>
    Task SendAsync(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a command that returns a response.
    /// <para>
    /// Use this when you expect a result from command execution, for example:
    /// <list type="bullet">
    /// <item><description>RegisterUserCommand → UserDto</description></item>
    /// <item><description>CreateInvoiceCommand → Guid</description></item>
    /// <item><description>GenerateTokenCommand → JwtToken</description></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="TResponse">The expected return type.</typeparam>
    /// <param name="command">The command instance to dispatch.</param>
    /// <param name="cancellationToken">A token for cancelling the operation.</param>
    /// <returns>The response returned by the command handler.</returns>
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        where TResponse : class;

    /// <summary>
    /// Sends a query that retrieves data and returns a response.
    /// <para>
    /// Queries must be side-effect free; examples:
    /// <list type="bullet">
    /// <item><description>GetOrderByIdQuery → OrderDto</description></item>
    /// <item><description>SearchProductsQuery → ProductListDto</description></item>
    /// <item><description>GetUserProfileQuery → UserProfileDto</description></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="TResponse">The type of the data returned from the query.</typeparam>
    /// <param name="query">The query instance containing lookup parameters.</param>
    /// <param name="cancellationToken">A token for cancelling the operation.</param>
    /// <returns>The response produced by the query handler.</returns>
    Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        where TResponse : class;
}
