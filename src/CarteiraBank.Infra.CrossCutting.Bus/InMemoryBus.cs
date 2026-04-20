using CarteiraBank.Domain.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CarteiraBank.Infra.CrossCutting.Bus;

public sealed class InMemoryBus(IServiceProvider serviceProvider) : IMediatorHandler
{
    public async Task<CommandResult> SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : Command
    {
        if (!command.IsValid())
        {
            var message = string.Join("; ", command.ValidationResult.Errors.Select(x => x.ErrorMessage));
            return CommandResult.Fail(message);
        }

        var handler = serviceProvider.GetRequiredService<IRequestHandler<TCommand>>();
        return await handler.HandleAsync(command, cancellationToken);
    }

    public async Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : Event
    {
        var handlers = serviceProvider.GetServices<INotificationHandler<TEvent>>();
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event, cancellationToken);
        }
    }
}
