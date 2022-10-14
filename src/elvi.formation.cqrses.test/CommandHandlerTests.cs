using System;
using System.Collections.Generic;
using elvi.formation.cqrses.test.CommandHandler;
using NUnit.Framework;

namespace elvi.formation.cqrses.test
{
    public class CommandHandlerTests
    {


        [Test]
        public void ShouldReadModelUpdateByCommandHandler()
        {

            var (dispatcher, commandHandler) = CreateCommandHandler();
            int folderId = 14;
            EstablishOrganisationsCommand command = new EstablishOrganisationsCommand(new List<string>() {"test"});
            
            commandHandler.Handler(folderId, command);

            Assert.IsTrue(dispatcher.Called);
        }

        private static (OrgannisationValidationListedEventHandlerDispatcher, EstablishOrganisationsCommandHandler) CreateCommandHandler()
        {
            IEventStore eventStore = new InMemoryEventStore();

            var dispatcher = new OrgannisationValidationListedEventHandlerDispatcher();

            Dictionary<Type, List<object>> dispatchers = new Dictionary<Type, List<object>>()
            {
                {typeof(OrgannisationValidationListed), new List<object>() {dispatcher}}
            };

            PubSub pubSub = new PubSub(eventStore, dispatchers);

            var commandHandler = new EstablishOrganisationsCommandHandler(eventStore, pubSub);
            return (dispatcher, commandHandler);
        }
    }
}