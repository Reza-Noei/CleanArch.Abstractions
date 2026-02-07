namespace CleanArch.Outbox.Abstractions;

/// <summary>
/// 
/// </summary>
internal interface IInboxMessagesService
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="messageId"></param>
    /// <param name="message"></param>
    /// <param name="occurredAt"></param>
    /// <returns></returns>
    Task<bool> TryInsertAsync<TMessage>(Guid messageId, TMessage message, DateTime occurredAt) where TMessage : class;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="messageType"></param>
    /// <param name="content"></param>
    /// <param name="occurredAt"></param>
    /// <returns></returns>
    Task<bool> TryInsertAsync(Guid messageId, string messageType, string content, DateTime occurredAt);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <returns></returns>
    Task RemoveAsync(Guid messageId);
}
