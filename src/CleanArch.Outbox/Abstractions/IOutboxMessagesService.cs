namespace CleanArch.Outbox.Abstractions;

/// <summary>
/// 
/// </summary>
internal interface IOutboxMessagesService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<OutboxRecord>> GetUnprocessedListAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <returns></returns>
    Task MarkAsProcessedAsync(Guid messageId);
}