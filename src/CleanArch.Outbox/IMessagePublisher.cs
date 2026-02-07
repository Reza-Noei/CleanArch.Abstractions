namespace CleanArch.Outbox;

/// <summary>
/// Abstraction for Message broker publisher.
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publish specific message on a Queue.
    /// </summary>
    /// <typeparam name="TMessage">Type of the Message</typeparam>
    /// <param name="message">Message payload</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : ;
}
