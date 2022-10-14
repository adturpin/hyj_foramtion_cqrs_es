using System;
using System.Collections.Generic;
using System.Linq;

namespace elvi.formation.cqrses.test
{
    public class PubSub
    {
        private readonly IEventStore _eventStore;
        private readonly Dictionary<Type, List<object>> _dispatchers;

        public PubSub(IEventStore eventStore, Dictionary<Type, List<object>> dispatchers)
        {
            _eventStore = eventStore;
            _dispatchers = dispatchers;
        }

        public void Publish(int streamId, List<DomainEvent> events)
        {
            _eventStore.AddEvents(streamId, events);

            foreach (var domainEvent in events)
            {
                if(!_dispatchers.ContainsKey(domainEvent.GetType()))
                    continue;
                
                Publish((dynamic)domainEvent, streamId);
            }
            
        }

        private void Publish<TEvent>(TEvent @event, int streamId) where TEvent : DomainEvent
        {
            foreach (var dispatcher in _dispatchers[typeof(TEvent)].Cast<IEventHandlerDispatcher<TEvent>>())
            {
                dispatcher.Handle(streamId, @event);
            }
        }
    }
}