using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRMUnitTestDemo.Tests.CRMHelpers
{
    public class ConnectionManager
    {
        public ConnectionManager()
        {
            ConnectToCrm();
        }

        public static OrganizationService OrgService = null;

        public static void ConnectToCrm()
        {
            try
            {
                var logIn = ConfigurationManager.AppSettings["CRM_User"];
                var pass = ConfigurationManager.AppSettings["CRM_Password"];
                var url = ConfigurationManager.AppSettings["CRM_URL"];

                var clientCredentials = new ClientCredentials();
                clientCredentials.UserName.UserName = logIn;
                clientCredentials.UserName.Password = pass;

                var connection = new CrmConnection
                {
                    ServiceUri = new Uri(url),
                    ClientCredentials = clientCredentials
                };

                OrgService = new OrganizationService(connection);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                throw e;
            }
        }
    }
}
