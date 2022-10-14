using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace elvi.formation.cqrses.test
{
    public class EventStoreTests
    {
        [Test]
        public void ShouldGetEventsFormInMemoryEventStore()
        {
            int streamId = 20;
            IEventStore eventStore = new InMemoryEventStore();
            eventStore.AddEvents(streamId, new List<DomainEvent>()
            {
                new OrgannisationValidationListed(
                    new List<string>()
                    {
                        "test1",
                        "test2"
                    }),
                new OrgannisationValidated("test2")
            });

            var events = eventStore.GetEvents(streamId);
            Assert.AreEqual(2, events.Count);
            Assert.IsInstanceOf<OrgannisationValidationListed>(events[0]);
            Assert.IsInstanceOf<OrgannisationValidated>(events[1]);

        }


        [Test]
        public void ShouldGetEventsFormInMemoryEventStoreFromMultiStream()
        {
            int firstStreamId = 20;
            IEventStore eventStore = new InMemoryEventStore();
            eventStore.AddEvents(firstStreamId, new List<DomainEvent>()
            {
                new OrgannisationValidationListed(
                    new List<string>()
                    {
                        "test1",
                        "test2"
                    }),
                new OrgannisationValidated("test2")
            },0);
            
            int secondStreamId = 22;
            eventStore.AddEvents(secondStreamId, new List<DomainEvent>()
            {
                new OrgannisationValidationListed(
                    new List<string>()
                    {
                        "balbla4",
                        "blabla2"
                    }),
                new VetoDisposed(),
                new FolderArchived("blabla2")
            },2);
            
            
            var events = eventStore.GetEvents(secondStreamId);
            
            
            Assert.AreEqual(3, events.Count);
            Assert.IsInstanceOf<OrgannisationValidationListed>(events[0]);
            Assert.IsInstanceOf<VetoDisposed>(events[1]);
            Assert.IsInstanceOf<FolderArchived>(events[2]);
        }
        
        [Test]
        public void ShouldntStoreEventsWithWrongSequenceVersionFormInMemoryEventStore()
        {
            int streamId = 20;
            IEventStore eventStore = new InMemoryEventStore();
            eventStore.AddEvents(streamId, new List<DomainEvent>()
            {
                new OrgannisationValidationListed(
                    new List<string>()
                    {
                        "test1",
                        "test2"
                    }),
                
            }, 0);

            Assert.Throws<Exception>(() =>
            {
                eventStore.AddEvents(streamId, new List<DomainEvent>()
                {
                    new OrgannisationValidated("test2")
                }, 0);
            });
        }
        
        
        [Test]
        public void ShouldStoreEventsWithGoodSequenceVersionFormInMemoryEventStore()
        {
            int streamId = 20;
            IEventStore eventStore = new InMemoryEventStore();
            eventStore.AddEvents(streamId, new List<DomainEvent>()
            {
                new OrgannisationValidationListed(
                    new List<string>()
                    {
                        "test1",
                        "test2"
                    }),
                
            }, 0);

            eventStore.AddEvents(streamId, new List<DomainEvent>()
            {
                new OrgannisationValidated("test2")
            }, 1);
            
            Assert.AreEqual(2, 
                ((InMemoryEventStore)eventStore).EventStoreData[streamId].Count);
        }


        [Test]
        public void ShouldSqlServer()
        {

            IEventStore eventStore = new SqlServerEventStore(@"Server=localhost;Database=hyj_formation;User Id=sa;Password=Your_password123;");
            ((SqlServerEventStore)eventStore).resetData();
            int firstStreamId = 20;
            eventStore.AddEvents(firstStreamId, new List<DomainEvent>()
            {
                new OrgannisationValidationListed(
                    new List<string>()
                    {
                        "test1",
                        "test2"
                    }),
                new OrgannisationValidated("test2")
            },0);
            
            int secondStreamId = 22;
            eventStore.AddEvents(secondStreamId, new List<DomainEvent>()
            {
                new OrgannisationValidationListed(
                    new List<string>()
                    {
                        "balbla4",
                        "blabla2"
                    }),
                new VetoDisposed(),
                new FolderArchived("blabla2")
            },2);
            
            
            var events = eventStore.GetEvents(secondStreamId);
            
            
            Assert.AreEqual(3, events.Count);

        }
    }
}