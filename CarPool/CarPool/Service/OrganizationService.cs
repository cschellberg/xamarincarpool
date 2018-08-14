using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Service
{
    public class OrganizationService
    {
        private static OrganizationService organizationService;

        public static OrganizationService getInstance()
        {
            if (organizationService == null)
            {
                organizationService = new OrganizationService();
            }
            return organizationService;
        }

        public List<Organization> GetOrganizations()
        {
            List<Organization> retList = new List<Organization>();
            Organization organization = new Organization();
            organization.Name = "Philadelphia Baha'i Community";
            organization.Id = 1;
            retList.Add(organization);
            return retList;
        }
    }
}
