using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRMUnitTestDemo.Tests.CRMHelpers
{
    public class DataGenerator
    {
        public static Entity RetriveEntityFakeData(string entityName, List<string> fieldsList, List<EntityField> mainCriteriaFields )
        {
            Entity fakeDataEntity = new Entity(entityName);

            QueryExpression fakeDataQueryExpression = new QueryExpression
            {
                EntityName = entityName,
                PageInfo = new PagingInfo { Count = 1, PageNumber = 1 },
                ColumnSet = new ColumnSet(true)
            };

            foreach (var field in fieldsList)
            {
                fakeDataQueryExpression.Criteria.AddCondition(field, ConditionOperator.NotNull);
            }
            fakeDataQueryExpression.Criteria.AddCondition(mainCriteriaFields[0].FieldName, ConditionOperator.Equal, mainCriteriaFields[0].FieldValue);

            EntityCollection baseFakeEntityCollection = ConnectionManager.OrgService.RetrieveMultiple(fakeDataQueryExpression);

            if (baseFakeEntityCollection.Entities.Count > 0)
                fakeDataEntity = baseFakeEntityCollection.Entities[0];
            return fakeDataEntity;
        }
    }
}
