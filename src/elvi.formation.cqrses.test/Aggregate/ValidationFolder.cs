using System.Collections.Generic;
using System.Linq;

namespace elvi.formation.cqrses.test
{
    public class ValidationFolder
    {
        private List<string> _awaitingOrganismeValidator;
        private List<string> _validatorOrganisations;
        private bool _isValided { get; set; }
        public ValidationFolder()
        {}
        
        public ValidationFolder(List<DomainEvent> seedingEvents)
        {
            _awaitingOrganismeValidator = new List<string>();
            _validatorOrganisations = new List<string>();
            Apply(seedingEvents);
        }

        private void Apply(List<DomainEvent> seedingEvents)
        {
            foreach (DomainEvent domainEvent in seedingEvents)
            {
                ApplyDomainEvent((dynamic)domainEvent);
            }
        }

        private void ApplyDomainEvent(FolderValidated domainEvent)
        {
            _isValided = true;
        }
        private void ApplyDomainEvent(OrgannisationValidated domainEvent)
        {
            if(_awaitingOrganismeValidator.Contains(domainEvent.OrganismeValidator))
                _awaitingOrganismeValidator.Remove(domainEvent.OrganismeValidator);
        }
        
        private void ApplyDomainEvent(OrgannisationValidationListed domainEvent)
        {
            _validatorOrganisations = domainEvent.ValidatorOrganisations.ToList();
            _awaitingOrganismeValidator = domainEvent.ValidatorOrganisations.ToList();
        }
        
        private void ApplyDomainEvent(FolderArchived domainEvent)
        {
            if(_awaitingOrganismeValidator.Contains(domainEvent.Archiver))
                _awaitingOrganismeValidator.Remove(domainEvent.Archiver);
        } 
        private void ApplyDomainEvent(VetoDisposed domainEvent)
        {
            // TODO: reset ?
        }
        
        public List<DomainEvent> ProcessCommand(EstablishOrganisationsCommand command)
        {
            return new List<DomainEvent>()
            {
                new OrgannisationValidationListed(command.List)
            };
        }

        public List<DomainEvent> ProcessCommand(OrgannisationValidationCommand command)
        {
            List<DomainEvent> result = new List<DomainEvent>();
            if (_awaitingOrganismeValidator.Contains(command.OrganismeValidator) )
            {
                var organnisationValidated = new OrgannisationValidated(command.OrganismeValidator);
                result.Add(organnisationValidated);
                ApplyDomainEvent(organnisationValidated);
            }
            
            if(!_isValided && !_awaitingOrganismeValidator.Any())
                result.Add(new FolderValidated());

            return result;
        }

        public List<DomainEvent> ProcessCommand(OrganisationApplyVetoCommand command)
        {
            if(_awaitingOrganismeValidator.Contains(command.Rejector))
            {
                var rejectEvent = new FolderArchived(command.Rejector);
                ApplyDomainEvent(rejectEvent);
                var vetoDeposted = new VetoDisposed();
                ApplyDomainEvent(vetoDeposted);
                return new List<DomainEvent>() {vetoDeposted, rejectEvent};
            }

            return Enumerable.Empty<DomainEvent>().ToList();
        }


    }
}