using System;
using CRMUnitTestDemo.Tests.CRMHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRMUnitTestDemo.Tests
{
    [TestClass]
    public class ConnectionTest
    {
        public ConnectionTest()
        {
            if(ConnectionManager.OrgService == null)
            ConnectionManager.ConnectToCrm();
        }

        [TestMethod]
        public void ConnectToCrmTest()
        {
            try
            {
                QueryExpression testConnectQueryExpression = new QueryExpression
                {
                    EntityName = "contact",
                    PageInfo = new PagingInfo { Count = 1, PageNumber = 1 },
                    ColumnSet = new ColumnSet("contactid")
                };

                EntityCollection result = ConnectionManager.OrgService.RetrieveMultiple(testConnectQueryExpression);
                
                Assert.AreEqual(1,result.Entities.Count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
