using System.Collections.Generic;

namespace elvi.formation.cqrses.test
{
    public interface IEventStore
    {
        void AddEvents(int streamId, List<DomainEvent> @event);
        void AddEvents(int streamId, List<DomainEvent> @event, int sequenceNumber);
        
        List<DomainEvent> GetEvents(int streamId);
    }
}