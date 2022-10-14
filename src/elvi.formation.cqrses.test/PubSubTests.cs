using System;
using System.Collections.Generic;
using System.Linq;
using elvi.formation.cqrses.test;
using NUnit.Framework;

namespace elvi.formation.cqrses.test
{
    public class PubSubTests
    {

        [Test]
        public void ShouldStoreEventsWhenPublishEvent()
        {
            int streamId = 50;
            List<DomainEvent> events = new List<DomainEvent>()
            {
                new OrgannisationValidationListed(new List<string>() {"organisme1"})
            };

            IEventStore eventStore = new TestEventStore();
            var dispatcher = new OrgannisationValidationListedEventHandlerDispatcher();

            Dictionary<Type, List<object>> dispatchers = new Dictionary<Type, List<object>>()
            {
                {typeof(OrgannisationValidationListed), new List<object>() {dispatcher}}
            };
            
            PubSub pubSub = new PubSub(eventStore, dispatchers);
            pubSub.Publish(streamId, events);

            Assert.AreEqual(1, ((TestEventStore) eventStore).EventStoreData[streamId].Count);
            Assert.IsTrue(dispatcher.Called);
        }
        
        
    }

    public class OrgannisationValidationListedEventHandlerDispatcher : IEventHandlerDispatcher<OrgannisationValidationListed>
    {
        public bool Called { get; set; } = false;
        
        public void Handle(int streamId, OrgannisationValidationListed domainEvent)
        {
            Called = true;
        }
    }

    public interface IEventHandlerDispatcher<TEvent> where TEvent: DomainEvent
    {
        void Handle(int streamId, TEvent domainEvent);
    }

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

    public class TestEventStore : IEventStore
    {
        public Dictionary<int, List<DomainEvent>> EventStoreData = new Dictionary<int, List<DomainEvent>>();
        
        public void AddEvents(int streamId, List<DomainEvent> @event)
        {
            if(!EventStoreData.ContainsKey(streamId))
                EventStoreData.Add(streamId, new List<DomainEvent>());
            
            EventStoreData[streamId].AddRange(@event);}    
        }

    public interface IEventStore
    {
        void AddEvents(int streamId, List<DomainEvent> @event);
    }
    
}
