using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FubuMVC.Core.View;
using StructureMap;

namespace FubuMVC.View.Spark
{
    public class SparkViewActivator : IViewActivator
    {
        IContainer _container;

        public SparkViewActivator(IContainer container)
        {
            _container = container;
        }

        public void Activate<T>(IFubuPage<T> page) where T : class
        {
            _container.BuildUp(page);
        }

        public void Activate(IFubuPage page)
        {
            _container.BuildUp(page);
        }
    }
}
