using System.Collections.Generic;

namespace elvi.formation.cqrses.test.ReadModel
{
    public class AwaitingOrganismeToValidateReadModel
    {

        public void ApplyEvent(OrgannisationValidationListed listed)
        {
            AwaitingOrganisationValidation = listed.ValidatorOrganisations.Count;
        }
        
        
        public void ApplyEvent(OrgannisationValidated validated)
        {
            AwaitingOrganisationValidation--;
        }

        public int AwaitingOrganisationValidation { get; private set; }
        
        
        
    }
}