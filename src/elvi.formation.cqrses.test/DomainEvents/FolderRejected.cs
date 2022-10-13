namespace elvi.formation.cqrses.test
{
    public record FolderRejected : DomainEvent
    {
        public string Rejector { get; }

        public FolderRejected(string rejector)
        {
            Rejector = rejector;
        }
    }
}