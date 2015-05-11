using System;
using CRMUnitTestDemo.Plugins;
using CRMUnitTestDemo.Tests.CRMHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Fakes;

namespace CRMUnitTestDemo.Tests.Plugins
{
    [TestClass]
    public class ContactParentPluginTest
    {
        [TestMethod]
        public void ExecuteTest()
        {
            ContactParentPlugin target = new ContactParentPlugin();

            var accountId = Guid.NewGuid();
            var previousNumber = 3;
            var expected = 4;
            var actual = 0;

            // IOrganizationService
            var service = new StubIOrganizationService();
            service.RetrieveStringGuidColumnSet = (entityName, id, columns) =>
            {
                return new Entity("account")
                {
                    Id = accountId,
                    Attributes = { { "numberofemployees", previousNumber } }
                };
            };
            service.UpdateEntity = (entity) =>
            {
                actual = entity.GetAttributeValue<int>("numberofemployees");
            };

            // IPluginExecutionContext
            var pluginExecutionContext = new StubIPluginExecutionContext();
            pluginExecutionContext.StageGet = () => { return 40; };
            pluginExecutionContext.MessageNameGet = () => { return "Create"; };
            pluginExecutionContext.PrimaryEntityNameGet = () => { return "contact"; };
            pluginExecutionContext.PostEntityImagesGet = () =>
            {
                return new EntityImageCollection
                { 
                    { "PostCreateImage", new Entity("contact") 
                                            {
                                                Attributes = { { "parentcustomerid", new EntityReference("account", accountId) } }
                                            }
                    }
                };
            };


            var serviceProvider = PluginsFakes.StubIServiceProvider(service, pluginExecutionContext);

            // Act
            //
            target.Execute(serviceProvider);

            //
            // Assert
            //
            Assert.AreEqual(expected, actual);
        }

    }
}
