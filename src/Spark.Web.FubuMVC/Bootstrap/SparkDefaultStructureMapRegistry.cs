using FubuMVC.Core;
using FubuMVC.View.Spark;
using Spark.Web.FubuMVC.Extensions;
using Spark.Web.FubuMVC.ViewLocation;
using IFubuViewActivator = FubuMVC.Core.View.IViewActivator;
namespace Spark.Web.FubuMVC.Bootstrap
{
    public class SparkDefaultStructureMapRegistry : FubuRegistry
    {
        public SparkDefaultStructureMapRegistry(bool debuggingEnabled, string controllerAssembly, SparkViewFactory viewFactory)
        {
            IncludeDiagnostics(debuggingEnabled);

            Applies.ToAssembly(controllerAssembly);

            Actions.IncludeTypesNamed(x => x.EndsWith("Controller"));

            Routes.IgnoreControllerNamespaceEntirely();

            Views.Facility(new SparkViewFacility(viewFactory, actionType => actionType.Name.EndsWith("Controller")))
                .TryToAttach(x => x.BySparkViewDescriptors(action => action.RemoveSuffix("Controller")));

            Services(s => s.SetServiceIfNone<IFubuViewActivator,SparkViewActivator>());
        }
    }
}