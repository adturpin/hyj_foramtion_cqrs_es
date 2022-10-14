using System;
using System.Collections.Generic;

namespace elvi.formation.cqrses.test
{
    public class InMemoryEventStore : IEventStore
    {
        public Dictionary<int, List<DomainEvent>> EventStoreData = new Dictionary<int, List<DomainEvent>>();
        
        public void AddEvents(int streamId, List<DomainEvent> @event)
        {
            if(!EventStoreData.ContainsKey(streamId))
                EventStoreData.Add(streamId, new List<DomainEvent>());
            
            EventStoreData[streamId].AddRange(@event);
            
        }

        public void AddEvents(int streamId, List<DomainEvent> @event, int sequenceNumber)
        {
            if(EventStoreData.ContainsKey(streamId) && 
               EventStoreData[streamId].Count != sequenceNumber)
                throw  new Exception("wrong sequence number");
            
            AddEvents(streamId, @event);
        }

        public List<DomainEvent> GetEvents(int streamId)
        {
            if(!EventStoreData.ContainsKey(streamId))
                return new List<DomainEvent>();

            return EventStoreData[streamId];
        }
    }
}