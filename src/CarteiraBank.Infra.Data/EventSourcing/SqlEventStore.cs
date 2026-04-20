using CarteiraBank.Domain.Core;
using CarteiraBank.Infra.Data.Context;

namespace CarteiraBank.Infra.Data.EventSourcing;

public sealed class SqlEventStore(CarteiraBankContext context) : IEventStore
{
    public async Task SaveAsync(StoredEvent storedEvent, CancellationToken cancellationToken = default)
    {
        context.EventStore.Add(new EventStoreSqlData
        {
            Id = storedEvent.Id,
            Type = storedEvent.Type,
            Data = storedEvent.Data,
            OccurredOn = storedEvent.OccurredOn,
            User = storedEvent.User
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}
