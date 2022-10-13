namespace elvi.formation.cqrses.test
{
    public record OrgannisationValidated : DomainEvent
    {
        public string OrganismeValidator { get; }

        public OrgannisationValidated(string organismeValidator)
        {
            OrganismeValidator = organismeValidator;
        }
    }
}