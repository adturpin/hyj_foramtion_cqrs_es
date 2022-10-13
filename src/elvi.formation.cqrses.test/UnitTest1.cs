using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace elvi.formation.cqrses.test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldEstablishOrganisationsListForValidationFolder()
        {

            EstablishOrganisationsCommand command = new EstablishOrganisationsCommand(new List<string>() {"organisme1", "organisme2"});
            
            ValidationFolder folder = new ValidationFolder();
            
            List<DomainEvent> events = folder.ProcessCommand(command);

            OrgannisationValidationListed expectedEvent = new OrgannisationValidationListed(new List<string>() {"organisme1", "organisme2"});
            
            
            Assert.AreEqual(1, events.Count);
            var firstevent = events.First();
            
            Assert.IsInstanceOf<OrgannisationValidationListed>(firstevent);
            
            CollectionAssert.AreEqual(expectedEvent.ValidatorOrganisations, ((OrgannisationValidationListed)firstevent).ValidatorOrganisations);

        }

        [Test]
        public void ShouldOrganisationsAValidatedForValidationFolder()
        {
            OrgannisationValidationCommand command = new OrgannisationValidationCommand("organisme1");
            
            ValidationFolder folder = new ValidationFolder(new List<DomainEvent>(){ new OrgannisationValidationListed(new List<string>() {"organisme1", "organisme2"})});
            var events = folder.ProcessCommand(command);
            
            Assert.AreEqual(1, events.Count);
            var lastEvent = events.Last();

            OrgannisationValidated expectedEvent = new OrgannisationValidated("organisme1");
            
            Assert.IsInstanceOf<OrgannisationValidated>(lastEvent);
            Assert.AreEqual(expectedEvent.OrganismeValidator, ((OrgannisationValidated)lastEvent).OrganismeValidator);
        }

        [Test]
        public void ShouldAllOrganisationsValidatorValidatedForValidationFolder()
        {
            OrgannisationValidationCommand command = new OrgannisationValidationCommand("organisme2");
            
            ValidationFolder folder = new ValidationFolder(new List<DomainEvent>()
            {
                new OrgannisationValidationListed(new List<string>() {"organisme1", "organisme2"}),
                new OrgannisationValidated("organisme1")
            });
            var events = folder.ProcessCommand(command);
            
            Assert.AreEqual(2, events.Count);
            
            OrgannisationValidated expectedEvent1 = new OrgannisationValidated("organisme2");
            FolderValidated expectedEvent2 = new FolderValidated();
            
            Assert.IsInstanceOf<OrgannisationValidated>(expectedEvent1);
            var validationEvent2 = events[0];
            Assert.AreEqual(expectedEvent1,validationEvent2);
            
            CollectionAssert.Contains(events, expectedEvent2);
        }

        [Test]
        public void ShouldSameOrganisationsValidatorValidatedTwiceForValidationFolder()
        {
            OrgannisationValidationCommand command = new OrgannisationValidationCommand("organisme1");
            
            ValidationFolder folder = new ValidationFolder(new List<DomainEvent>()
            {
                new OrgannisationValidationListed(new List<string>() {"organisme1", "organisme2"}),
                new OrgannisationValidated("organisme1")
            });
            var events = folder.ProcessCommand(command);
            
            Assert.AreEqual(0, events.Count);
        }
    }

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
    }
    
    
    
    
}