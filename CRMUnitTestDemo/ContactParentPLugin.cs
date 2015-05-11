using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRMUnitTestDemo.Plugins
{
    public class ContactParentPlugin : IPlugin
    {
        private readonly string postImageAlias = "PostCreateImage";

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context service from the service provider.
            var pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (pluginExecutionContext.Stage != 40 || pluginExecutionContext.MessageName != "Create" ||
                pluginExecutionContext.PrimaryEntityName != "contact") return;

            // Obtain the tracing service from the service provider.
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the Organization Service factory service from the service provider
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // Use the factory to generate the Organization Service.
            var organizationService = factory.CreateOrganizationService(pluginExecutionContext.UserId);

            Entity postImageEntity = (pluginExecutionContext.PostEntityImages != null && pluginExecutionContext.PostEntityImages.Contains(postImageAlias)) ? pluginExecutionContext.PostEntityImages[postImageAlias] : null;

            // when a contact is created with parent account, increase number of employees for that account
            if (postImageEntity == null || !postImageEntity.Contains("parentcustomerid")) return;
            EntityReference parentCustomer = postImageEntity.GetAttributeValue<EntityReference>("parentcustomerid");

            if (parentCustomer == null || parentCustomer.LogicalName.ToLowerInvariant() != "account") return;
            tracingService.Trace("Parent account id: {0}.", parentCustomer.Id);

            Entity parentAccount = organizationService.Retrieve("account", parentCustomer.Id, new ColumnSet("numberofemployees"));

            int numberOfEmployees = 0;
            if (parentAccount.Contains("numberofemployees"))
            {
                numberOfEmployees = parentAccount.GetAttributeValue<int>("numberofemployees");
            }

            parentAccount["numberofemployees"] = numberOfEmployees + 1;

            organizationService.Update(parentAccount);
        }
    }
}
