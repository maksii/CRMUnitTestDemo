using System;
using System.Collections.Generic;
using System.Linq;
using CRMUnitTestDemo.Tests.CRMHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRMUnitTestDemo.Tests.CRUD
{
    [TestClass]
    public class PropertyTest
    {

        public PropertyTest()
        {
            if(ConnectionManager.OrgService == null)
            ConnectionManager.ConnectToCrm();
        }

        private Guid _propertyGuid = Guid.Empty;
        private Guid _fakeGuidData = Guid.Empty;
        private string _valueToCreate = String.Empty;

        private string _currentValue = String.Empty;
        private string _valueToUpdate = String.Empty;
        private string _updatedValue = String.Empty;

           [TestMethod]
           public void PropertyEntityTest()
           {
               CreateProperty();

               Assert.AreNotEqual(Guid.Empty, _propertyGuid);
               Assert.AreEqual(_fakeGuidData, _propertyGuid);

               UpdateProperty();
               Assert.AreEqual(_valueToCreate, _currentValue);
               Assert.AreNotEqual(_currentValue, _updatedValue);
               Assert.AreEqual(_updatedValue, _valueToUpdate);
           }

           public void UpdateProperty()
           {
               Entity propertyEntity = ConnectionManager.OrgService.Retrieve("awx_property", _propertyGuid, new ColumnSet("awx_address"));
               _currentValue = propertyEntity["awx_address"].ToString();
               _valueToUpdate = String.Format("UnitTests value: {0}", DateTime.Now);
               propertyEntity["awx_address"] = _valueToUpdate;

               ConnectionManager.OrgService.Update(propertyEntity);
               Entity propertyEntityUpd = ConnectionManager.OrgService.Retrieve("awx_property", _propertyGuid, new ColumnSet("awx_address"));

               _updatedValue = propertyEntityUpd["awx_address"].ToString();

           }

        private void CreateProperty()
        {
            List<string> fieldList = new List<string> { "awx_longitude", "awx_latitude", "awx_propertytype", "awx_countryid" };
            List<EntityField> fieldStatus = new List<EntityField>{new EntityField
               {
                   FieldName = "statecode",
                   FieldValue = 0
               }};

            _fakeGuidData = Guid.NewGuid();
            Entity propertyFakeData = DataGenerator.RetriveEntityFakeData("awx_property", fieldList, fieldStatus);
            propertyFakeData.Id = _fakeGuidData;
            propertyFakeData["awx_propertyid"] = _fakeGuidData;
            _valueToCreate = String.Format("UnitTestRecord value: {0}", DateTime.Now);
            propertyFakeData["awx_address"] = _valueToCreate;
            _propertyGuid = ConnectionManager.OrgService.Create(propertyFakeData);

        }
    }
}
