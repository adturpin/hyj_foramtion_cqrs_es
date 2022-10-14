using System.Collections.Generic;

namespace elvi.formation.cqrses.test
{
    public class TestEventStore : IEventStore
    {
        public Dictionary<int, List<DomainEvent>> EventStoreData = new Dictionary<int, List<DomainEvent>>();
        
        public void AddEvents(int streamId, List<DomainEvent> @event)
        {
            if(!EventStoreData.ContainsKey(streamId))
                EventStoreData.Add(streamId, new List<DomainEvent>());
            
            EventStoreData[streamId].AddRange(@event);}

        public List<DomainEvent> GetEvents(int streamId)
        {
            throw new System.NotImplementedException();
        }
    }
}