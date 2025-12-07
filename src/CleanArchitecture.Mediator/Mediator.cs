using CleanArchitecture.Mediator.Contracts;

namespace CleanArchitecture.Mediator;

/// <summary>
/// Default implementation of the mediator pattern.
/// Routes commands and queries to their corresponding handlers
/// and executes registered pipeline behaviors.
/// </summary>
public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new mediator using the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">
    /// The DI service provider used to resolve handlers and pipeline behaviors.
    /// </param>
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc/>
    public async Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        await SendInternalAsync<Unit>(command, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        where TResponse : class
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        return await SendInternalAsync<TResponse>(command, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        where TResponse : class
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        return await QueryInternalAsync<TResponse>(query, cancellationToken);
    }

    /// <summary>
    /// Internal dispatch logic that executes pipeline behaviors and the final handler.
    /// </summary>
    private async Task<TResponse> SendInternalAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken) where TResponse : class
    {
        Type requestType = command.GetType();
        Type responseType = typeof(TResponse);

        // Resolve handler
        Type handlerType = GetHandlerType(requestType, responseType);

        ICommandHandler<ICommand<TResponse>, TResponse>? handler =
            _serviceProvider.GetService(handlerType) as ICommandHandler<ICommand<TResponse>, TResponse>;

        if (handler == null)
            throw new InvalidOperationException($"Handler not found for {requestType.Name}");

        // Resolve all pipeline behaviors
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(typeof(ICommand<TResponse>), typeof(TResponse));

        var behaviors = (_serviceProvider.GetService(typeof(IEnumerable<>)
            .MakeGenericType(behaviorType)) as IEnumerable<object> ?? Enumerable.Empty<object>())
            .Cast<IPipelineBehavior<ICommand<TResponse>, TResponse>>();

        // Build pipeline chain
        Func<Task<TResponse>> handlerDelegate = () => handler.HandleAsync(command, cancellationToken);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.HandleAsync(command, cancellationToken, next);
        }

        // Execute pipeline
        return await handlerDelegate();

    }

    private async Task<TResponse> QueryInternalAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken) where TResponse : class
    {
        Type requestType = query.GetType();
        Type responseType = typeof(TResponse);

        // Resolve handler
        Type handlerType = GetHandlerType(requestType, responseType);
        
        IQueryHandler<IQuery<TResponse>, TResponse>? handler = 
            _serviceProvider.GetService(handlerType) as IQueryHandler<IQuery<TResponse>, TResponse>;
        
        if (handler == null)
            throw new InvalidOperationException($"Handler not found for {requestType.Name}");

        // Resolve all pipeline behaviors
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(typeof(IQuery<TResponse>), typeof(TResponse));

        var behaviors = (_serviceProvider.GetService(typeof(IEnumerable<>)
            .MakeGenericType(behaviorType)) as IEnumerable<object> ?? Enumerable.Empty<object>())
            .Cast<IPipelineBehavior<IQuery<TResponse>, TResponse>>();
        
        // Build pipeline chain
        Func<Task<TResponse>> handlerDelegate = () => handler.HandleAsync(query, cancellationToken);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.HandleAsync(query, cancellationToken, next);
        }

        // Execute pipeline
        return await handlerDelegate();
    }



    private static Type GetHandlerType(Type requestType, Type responseType)
    {
        if (typeof(ICommand).IsAssignableFrom(requestType) || typeof(ICommand<>).IsAssignableFrom(requestType))
            return typeof(ICommandHandler<,>).MakeGenericType(requestType, responseType);

        if (typeof(IQuery<>).IsAssignableFrom(requestType))
            return typeof(IQueryHandler<,>).MakeGenericType(requestType, responseType);

        throw new InvalidOperationException($"Unsupported request type {requestType.Name}");
    }
}