namespace IdentityServer.Domain.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents);
}
