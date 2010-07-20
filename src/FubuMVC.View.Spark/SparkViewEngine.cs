using System;
using System.IO;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View;
using Spark;
using Spark.FileSystem;

namespace FubuMVC.View.Spark
{
    public class SparkViewEngine<T> : IViewEngine<T> where T : class
    {
        private readonly IOutputWriter _outputWriter;
        readonly ISparkViewBuilder _builder;

        public SparkViewEngine(IOutputWriter outputWriter,ISparkViewBuilder builder)
        {
            _outputWriter = outputWriter;
            _builder = builder;
        }

        public void RenderView(ViewPath viewPath, Action<T> configureView)
        {
            var engine = new SparkViewEngine { DefaultPageBaseType = typeof(FubuSparkView).FullName };

            var descriptor = new SparkViewDescriptor().AddTemplate(viewPath.ViewName);

            var view = (IFubuSparkView)engine.CreateInstance(descriptor);
            _builder.Build(view);

            var configurableView = view as T;
            if (configurableView != null)
            {
                configureView(configurableView);
            }
            string output = null;
            using (var writer = new StringWriter())
            {
                try
                {

                    view.RenderView(writer);
                }
                finally
                {
                    engine.ReleaseInstance(view);
                }

                output = writer.ToString();
            }


            _outputWriter.Write("text/html", output);
        }
    }
}