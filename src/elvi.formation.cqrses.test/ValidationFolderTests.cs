using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace elvi.formation.cqrses.test
{
    public class ValidationFolderTests
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
        
        
        [Test]
        public void ShouldAllOrganisationsValidatorValidatedForValidationFolder2()
        {
            OrgannisationValidationCommand command = new OrgannisationValidationCommand("organisme1");
            
            ValidationFolder folder = new ValidationFolder(new List<DomainEvent>()
            {
                new OrgannisationValidationListed(new List<string>() {"organisme1", "organisme2"}),
                new OrgannisationValidated("organisme2")
            });
            var events = folder.ProcessCommand(command);
            
            Assert.AreEqual(2, events.Count);
            
            OrgannisationValidated expectedEvent1 = new OrgannisationValidated("organisme1");
            FolderValidated expectedEvent2 = new FolderValidated();
            
            Assert.IsInstanceOf<OrgannisationValidated>(expectedEvent1);
            var validationEvent2 = events[0];
            Assert.AreEqual(expectedEvent1,validationEvent2);
            
            CollectionAssert.Contains(events, expectedEvent2);
        }
        
        [Test]
        public void ShouldOrganisationsValidatorApplyVetoForValidationFolder()
        {
            OrganisationApplyVetoCommand command = new OrganisationApplyVetoCommand("organisme1");
            
            ValidationFolder folder = new ValidationFolder(new List<DomainEvent>()
            {
                new OrgannisationValidationListed(new List<string>() {"organisme1", "organisme2"}),
            });
            var events = folder.ProcessCommand(command);
            
            Assert.AreEqual(2, events.Count);
            
            // var firstEvent = events.First();
            // Assert.IsInstanceOf<FolderArchived>(firstEvent);
            // Assert.AreEqual("organisme1", ((FolderArchived)firstEvent).Archiver);
            
            var lastEvent = events.Last();
            Assert.IsInstanceOf<FolderArchived>(lastEvent);
            Assert.AreEqual("organisme1", ((FolderArchived)lastEvent).Archiver);

        }
        
    }
}