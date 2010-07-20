using System;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using FubuMVC.Core.View;
using HtmlTags;
using Microsoft.Practices.ServiceLocation;
using Spark;
using FubuCore.Util;

namespace FubuMVC.View.Spark
{

    public abstract class FubuSparkPageView : AbstractSparkView, IFubuPage
    {
           private readonly Cache<Type, object> _services = new Cache<Type, object>();

           public FubuSparkPageView()
        {
            _services.OnMissing = type => { return ServiceLocator.GetInstance(type); };
        }

        public IServiceLocator ServiceLocator { get; set; }

        public T Get<T>()
        {
            return (T) _services[typeof (T)];
        }

        public T GetNew<T>()
        {
            return ServiceLocator.GetInstance<T>();
        }

        public IUrlRegistry Urls
        {
            get { return Get<IUrlRegistry>(); }
        }

        string IFubuPage.ElementPrefix { get; set; }

        public HtmlTag Tag(string tagName)
        {
            return new HtmlTag(tagName);
        }
    }

    public abstract class FubuSparkView<T> : FubuSparkPageView, IFubuSparkView, IFubuPage<T> where T : class
    {
        public T Model { get; private set; }
     
        public void SetModel(IFubuRequest request)
        {
            Model = request.Get<T>();
        }

        public void SetModel(object model)
        {
            Model = model as T;
        }

     
    }

    public interface IFubuSparkView : ISparkView, IFubuPage { }

    public abstract class FubuSparkView : FubuSparkView<object>
    {
    }
}
