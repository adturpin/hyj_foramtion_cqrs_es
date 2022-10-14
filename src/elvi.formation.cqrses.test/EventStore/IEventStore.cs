using System.Collections.Generic;

namespace elvi.formation.cqrses.test
{
    public interface IEventStore
    {
        void AddEvents(int streamId, List<DomainEvent> @event);
    }
}