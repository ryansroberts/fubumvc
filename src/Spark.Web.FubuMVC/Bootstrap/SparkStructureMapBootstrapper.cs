using System.Web.Routing;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using FubuMVC.StructureMap;
using FubuMVC.View.Spark;
using Microsoft.Practices.ServiceLocation;
using Spark.Web.FubuMVC.ViewCreation;
using StructureMap;

namespace Spark.Web.FubuMVC.Bootstrap
{
    public class SparkStructureMapBootstrapper
    {
        private readonly RouteCollection _routes;

        private SparkStructureMapBootstrapper(RouteCollection routes)
        {
            _routes = routes;
        }

        public static void Bootstrap(RouteCollection routes, FubuRegistry fubuRegistry)
        {
            new SparkStructureMapBootstrapper(routes).BootstrapStructureMap(fubuRegistry);
        }

        private void BootstrapStructureMap(FubuRegistry fubuRegistry)
        {
            UrlContext.Reset();

            ObjectFactory.Initialize(x =>
                {

                    x.For<ISparkSettings>()
                        .Use<SparkSettings>();
                    
                    x.For<SparkViewFactory>()
                        .Singleton();

                    x.For<IServiceLocator>()
                        .Use<StructureMapServiceLocator>();

                    x.For<ISessionState>()
                        .Use<SimpleSessionState>();

                    x.For(typeof (ISparkViewRenderer<>))
                        .Use(typeof (SparkViewRenderer<>));

                    x.SetAllProperties(s =>
                        {
                            s.Matching(p => p.DeclaringType == typeof(FubuSparkPageView));
                            s.OfType<IServiceLocator>();
                        });
                });

            var bootstrapper = new StructureMapBootstrapper(ObjectFactory.Container, fubuRegistry);
            bootstrapper.Bootstrap(_routes);
        }
    }
}