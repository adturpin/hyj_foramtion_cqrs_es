namespace elvi.formation.cqrses.test
{
    public class OrganisationApplyVetoCommand : DomainCommand
    {
        public string Rejector { get; }

        public OrganisationApplyVetoCommand(string rejector)
        {
            Rejector = rejector;
        }
    }
}