using System.Collections.Generic;
using NUnit.Framework;
using Spark.Web.FubuMVC.Configuration;

namespace Spark.Web.FubuMVC.Tests.Configuration
{
    [TestFixture]
    public class SparkSettingsMergeTester
    {
        SparkSettingsMerger Merger;

        [SetUp]
        public void Init() {Merger = new SparkSettingsMerger(); }
    
        [Test]
        public void SettingsAreMergedRightToLeft()
        {
            var left = new SparkSettings();
            var right = new SparkSettings();

            right.NullBehaviour = NullBehaviour.Strict;
            right.PageBaseType = "test";
            right.Prefix = "test";
            right.StatementMarker = "T";
            right.AutomaticEncoding = true;
            right.AddResourceMapping("test", "test");
            right.AddAssembly("test");
            right.AddNamespace("test");
            right.AddViewFolder(typeof (int), new Dictionary<string, string>());

            Merger.Merge(left, right);



        }
    }
}