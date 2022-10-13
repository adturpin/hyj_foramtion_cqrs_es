namespace elvi.formation.cqrses.test
{
    public class OrgannisationRejectCommand : DomainCommand
    {
        public string Rejector { get; }

        public OrgannisationRejectCommand(string rejector)
        {
            Rejector = rejector;
        }
    }
}