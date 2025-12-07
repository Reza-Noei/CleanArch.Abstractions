namespace CleanArchitecture.Mediator.Contracts;

/// <summary>
/// Represents the base abstraction for all messages exchanged through the Mediator.
/// </summary>
/// <remarks>
/// This interface acts as a common contract for commands, queries, and any other
/// message types used in a CQRS or Mediator-based architecture.
///
/// It does not define any members by itself and serves primarily as a marker 
/// interface to enable pipeline behaviors such as logging, validation, 
/// authorization, transactions, or tracing.
/// </remarks>
public interface IMessage
{

}
