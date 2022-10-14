using System;
using System.Collections.Generic;

namespace elvi.formation.cqrses.test.ReadModel
{
    public class AwaitingOrganismeToValidateReadModelHandler
    {
        private readonly IDictionary<int, AwaitingOrganismeToValidateReadModel> _repository;

        public AwaitingOrganismeToValidateReadModelHandler(IDictionary<int, AwaitingOrganismeToValidateReadModel> repository)
        {
            _repository = repository;
        }


        private void Handler(int folderId, Func<AwaitingOrganismeToValidateReadModel,AwaitingOrganismeToValidateReadModel> apply)
        {
            if (!_repository.ContainsKey(folderId))
                _repository[folderId] = new AwaitingOrganismeToValidateReadModel();
            
            var folder = _repository[folderId];
            _repository[folderId] = apply(folder);
        }
        
        
        public void Handler(int folderId, OrgannisationValidated validationListed)
        {
            Handler(folderId, (readModel) =>
            {
                readModel.ApplyEvent(validationListed);
                return readModel;
            });
        }
        
        
        public void Handler(int folderId, OrgannisationValidationListed validationListed)
        {
            Handler(folderId, (readModel) =>
            {
                readModel.ApplyEvent(validationListed);
                return readModel;
            });
        }
    }
}