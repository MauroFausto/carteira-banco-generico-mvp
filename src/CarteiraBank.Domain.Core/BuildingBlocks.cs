using FluentValidation.Results;

namespace CarteiraBank.Domain.Core;

public abstract class Entity
{
    private readonly List<Event> _events = [];

    public Guid Id { get; protected set; } = Guid.NewGuid();

    public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

    public void AddEvent(Event @event) => _events.Add(@event);

    public void ClearEvents() => _events.Clear();

    public override bool Equals(object? obj)
    {
        if (obj is not Entity compareTo)
        {
            return false;
        }

        if (ReferenceEquals(this, compareTo))
        {
            return true;
        }

        return Id.Equals(compareTo.Id);
    }

    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
}

public abstract class Command
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public ValidationResult ValidationResult { get; protected set; } = new();

    public abstract bool IsValid();
}

public abstract class Event
{
    protected Event()
    {
        Timestamp = DateTime.UtcNow;
    }

    public DateTime Timestamp { get; }
}

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}

public interface IRepository<T> where T : Entity
{
    IUnitOfWork UnitOfWork { get; }
}

public interface IEventStore
{
    Task SaveAsync(StoredEvent storedEvent, CancellationToken cancellationToken = default);
}

public interface IMediatorHandler
{
    Task<CommandResult> SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : Command;

    Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : Event;
}

public interface IRequestHandler<in TCommand>
    where TCommand : Command
{
    Task<CommandResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface INotificationHandler<in TEvent>
    where TEvent : Event
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}

public sealed record StoredEvent(
    Guid Id,
    string Type,
    string Data,
    DateTime OccurredOn,
    string? User);

public sealed record CommandResult(bool Success, string Message, Guid AggregateId)
{
    public static CommandResult Ok(Guid aggregateId, string message = "Comando executado com sucesso.")
        => new(true, message, aggregateId);

    public static CommandResult Fail(string message)
        => new(false, message, Guid.Empty);
}
