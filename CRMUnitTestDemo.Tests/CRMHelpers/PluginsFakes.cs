using System.Diagnostics;
using System.Fakes;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Fakes;

namespace CRMUnitTestDemo.Tests.CRMHelpers
{
   public class PluginsFakes
    {
       public static StubIServiceProvider StubIServiceProvider(StubIOrganizationService service,
           StubIPluginExecutionContext pluginExecutionContext)
       {
           // ITracingService
           var tracingService = new StubITracingService
           {
               TraceStringObjectArray = (f, o) => { Debug.WriteLine(f, o); }
           };

           // IOrganizationServiceFactory
           var factory = new StubIOrganizationServiceFactory
           {
               CreateOrganizationServiceNullableOfGuid = id => service
           };

           // IServiceProvider
           var serviceProvider = new StubIServiceProvider
           {
               GetServiceType = t =>
               {
                   if (t == typeof(IPluginExecutionContext))
                   {
                       return pluginExecutionContext;
                   }
                   if (t == typeof(ITracingService))
                   {
                       return tracingService;
                   }
                   return t == typeof(IOrganizationServiceFactory) ? factory : null;
               }
           };
           return serviceProvider;
       }
    }
}
