using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FubuMVC.Core.View;
using StructureMap;

namespace FubuMVC.View.Spark
{
    public class StructuremapSparkViewBuilder : ISparkViewBuilder
    {
        IContainer _container;

        public StructuremapSparkViewBuilder(IContainer container)
        {
            _container = container;
        }

        public void Build(IFubuPage page)
        {
            _container.BuildUp(page);
        }
    }

    public class NulloSparkViewBuilder : ISparkViewBuilder
    {
        public void Build(IFubuPage page)
        {
        }
   }


    public interface ISparkViewBuilder
    {
        void Build(IFubuPage page);
    }
}
