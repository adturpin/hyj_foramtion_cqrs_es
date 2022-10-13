using System.Collections.Generic;

namespace elvi.formation.cqrses.test
{
    public record OrgannisationValidationListed : DomainEvent
    {
        public List<string> ValidatorOrganisations { get; }

        public OrgannisationValidationListed(List<string> validatorOrganisations)
        {
            ValidatorOrganisations = validatorOrganisations;
        }
    }
}