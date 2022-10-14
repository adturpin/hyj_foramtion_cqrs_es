namespace elvi.formation.cqrses.test.CommandHandler
{
    
    public class EstablishOrganisationsCommandHandler : ICommandHandler<EstablishOrganisationsCommand>
    {
        private readonly IEventStore _eventStore;
        private readonly PubSub _pubSub;

        public EstablishOrganisationsCommandHandler(IEventStore eventStore, PubSub pubSub)
        {
            _eventStore = eventStore;
            _pubSub = pubSub;
        }
        
        public void Handler(int folderId, EstablishOrganisationsCommand command)
        {
            var events = _eventStore.GetEvents(folderId);
            var folder = new ValidationFolder(events);
            var processEvents = folder.ProcessCommand(command);
            _pubSub.Publish(folderId, processEvents);
        }
    }

    public interface ICommandHandler<T> where T : DomainCommand
    {
        void Handler(int folderId,T command);
    }
}