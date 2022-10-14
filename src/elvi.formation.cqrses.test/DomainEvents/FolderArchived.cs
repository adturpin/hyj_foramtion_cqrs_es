namespace elvi.formation.cqrses.test
{
    public record FolderArchived : DomainEvent
    {
        public string Archiver { get; }

        public FolderArchived(string archiver)
        {
            Archiver = archiver;
        }
    }
}