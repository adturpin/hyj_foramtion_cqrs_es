using System.Collections.Generic;
using elvi.formation.cqrses.test.ReadModel;
using NUnit.Framework;

namespace elvi.formation.cqrses.test
{
    public class ReadModelTests
    {
        [Test]
        public void ShouldGetAwaitingOrganismeNumberForInitalFolderValidation()
        {
            AwaitingOrganismeToValidateReadModel readModel = new AwaitingOrganismeToValidateReadModel();
            
            Assert.AreEqual(0,readModel.AwaitingOrganisationValidation);
        }
        
        
        [Test]
        public void ShouldGetAwaitingOrganismeNumberForFolderValidation()
        {
           AwaitingOrganismeToValidateReadModel readModel = new AwaitingOrganismeToValidateReadModel();
            
            readModel.ApplyEvent(new OrgannisationValidationListed(new List<string>(){ "organisame1" }));
            
            Assert.AreEqual(1,readModel.AwaitingOrganisationValidation);
        }


        [Test]
        public void ShouldGetAwaitingOrganismeNumberForFolderValidationFromHandler()
        {
            var repository = new Dictionary<int, AwaitingOrganismeToValidateReadModel>()
            {
                {1, new AwaitingOrganismeToValidateReadModel() }
            };
            
            AwaitingOrganismeToValidateReadModelHandler handler = new AwaitingOrganismeToValidateReadModelHandler(
                repository);
            handler.Handler(1, new OrgannisationValidationListed(new List<string>(){ "organisame1" }));


            Assert.AreEqual(1, repository[1].AwaitingOrganisationValidation);
        }
    }
}