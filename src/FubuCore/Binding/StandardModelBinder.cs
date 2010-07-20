using System;
using System.Linq;

namespace FubuCore.Binding
{
    public class StandardModelBinder : IModelBinder
    {
        private readonly IPropertyBinderCache _propertyBinders;
        private readonly ITypeDescriptorCache _typeCache;

        public StandardModelBinder(IPropertyBinderCache propertyBinders, ITypeDescriptorCache typeCache)
        {
            _propertyBinders = propertyBinders;
            _typeCache = typeCache;
        }

        public bool Matches(Type type)
        {
            return type.GetConstructors().Count(x => x.GetParameters().Length == 0) == 1;
        }

        public object Bind(Type type, IBindingContext context)
        {
            object instance = Activator.CreateInstance(type);
            Bind(type, instance, context);

            return instance;
        }

        public void Bind(Type type, object instance, IBindingContext context)
        {
            context.StartObject(instance);
            populate(type, context);
            context.FinishObject();
        }

        // Only exists for easier testing
        public void Populate(object target, IBindingContext context)
        {
            context.StartObject(target);
            populate(target.GetType(), context);
            context.FinishObject();
        }

        private void populate(Type type, IBindingContext context)
        {
            _typeCache.ForEachProperty(type, prop =>
            {
                IPropertyBinder binder = _propertyBinders.BinderFor(prop);
                binder.Bind(prop, context);
            });
        }
    }
}