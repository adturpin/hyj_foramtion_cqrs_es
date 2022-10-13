using System.Collections.Generic;

namespace elvi.formation.cqrses.test
{
    public class EstablishOrganisationsCommand : DomainCommand
    {
        public List<string> List { get; }

        public EstablishOrganisationsCommand(List<string> list)
        {
            List = list;
        }
    }
}