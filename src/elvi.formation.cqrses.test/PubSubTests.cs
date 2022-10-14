using System;
using System.Collections.Generic;
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

            IEventStore eventStore = new InMemoryEventStore();
            var dispatcher = new OrgannisationValidationListedEventHandlerDispatcher();

            Dictionary<Type, List<object>> dispatchers = new Dictionary<Type, List<object>>()
            {
                {typeof(OrgannisationValidationListed), new List<object>() {dispatcher}}
            };
            
            PubSub pubSub = new PubSub(eventStore, dispatchers);
            pubSub.Publish(streamId, events);

            Assert.AreEqual(1, ((InMemoryEventStore) eventStore).EventStoreData[streamId].Count);
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
}

