namespace elvi.formation.cqrses.test
{
    public class OrgannisationValidationCommand : DomainCommand
    {
        public string OrganismeValidator { get; }

        public OrgannisationValidationCommand(string organismeValidator)
        {
            OrganismeValidator = organismeValidator;
        }
    }
}