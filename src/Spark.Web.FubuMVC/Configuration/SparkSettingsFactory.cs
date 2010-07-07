using System;
using FubuMVC.Core.Registration.Nodes;

namespace Spark.Web.FubuMVC.Configuration
{

    public class SparkSettingsMerger
    {
        public ISparkSettings Merge(ISparkSettings left, ISparkSettings right)
        {
            return left;
        }
    }

    public class SparkSettingsFactory : ISparkSettingsFactory
    {
        public ISparkSettings GetSettingsForActionCall(ActionCall call)
        {
            return new SparkSettings();
        }

        public ISparkSettings GetSettings()
        {
            return new SparkSettings();
        }
    }

    public interface ISparkSettingsFactory
    {
        ISparkSettings GetSettingsForActionCall(ActionCall call);
        ISparkSettings GetSettings();
    }
}