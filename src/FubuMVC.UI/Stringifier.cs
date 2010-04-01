using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Util;

namespace FubuMVC.UI
{
    public class Stringifier
    {
        private readonly Cache<Type, Func<object, string>> _converters = new Cache<Type, Func<object, string>>();

        private readonly List<StringifierStrategy> _strategies = new List<StringifierStrategy>();
        private readonly List<PropertyOverrideStrategy> _overrides = new List<PropertyOverrideStrategy>();
        public Stringifier()
        {
            _converters.OnMissing = type =>
            {
                if (type.IsNullable())
                {
                    return instance =>
                    {
                        return instance == null ? string.Empty : _converters[type.GetInnerTypeFromNullable()](instance);
                    };
                }
                var strategy = _strategies.FirstOrDefault(x => x.Matches(type));
                return strategy == null ? toString : strategy.StringFunction;
            };
        }

        private static string toString(object value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public string GetStringRequest(GetStringRequest request)
        {
            var rawValue = request.RawValue;

            if (rawValue == null || (rawValue as String) == string.Empty) return string.Empty;
            var propertyOverride = _overrides.FirstOrDefault(o => o.Matches(request.Property));
            return propertyOverride != null
                ? propertyOverride.StringFunction(request) 
                : GetString(rawValue);
        }


        public string GetString(object rawValue)
        {
            if( rawValue == null || (rawValue as String) == string.Empty ) return string.Empty;

            return _converters[rawValue.GetType()](rawValue);
        }

        public void IfIsType<T>(Func<T, string> display)
        {
            _strategies.Add(new StringifierStrategy
            {
                Matches = type => type == typeof (T),
                StringFunction = o => display((T) o)
            });
        }

        public void IfCanBeCastToType<T>(Func<T, string> display)
        {
            _strategies.Add(new StringifierStrategy
            {
                Matches = t => t.CanBeCastTo<T>(),
                StringFunction = o => display((T)o)
            });
        }

        public class StringifierStrategy
        {
            public Func<Type, bool> Matches;
            public Func<object, string> StringFunction;
        }

        public class PropertyOverrideStrategy
        {
            public Func<PropertyInfo, bool> Matches;
            public Func<GetStringRequest, string> StringFunction;
        }

        public void IfPropertyMatches(Func<PropertyInfo, bool> matches, Func<object, string> display)
        {
            _overrides.Add(new PropertyOverrideStrategy
            {
                Matches = matches, 
                StringFunction = (r) => display(r.RawValue)
            });
        }

        public void IfPropertyMatches(Func<PropertyInfo, bool> matches, Func<GetStringRequest, string> display)
        {
            _overrides.Add(new PropertyOverrideStrategy
            {
                Matches = matches,
                StringFunction = display
            });
        }

        public void IfPropertyMatches<T>(Func<PropertyInfo, bool> matches, Func<T, string> display)
        {
            IfPropertyMatches(p => p.PropertyType.CanBeCastTo<T>() && matches(p), r => display((T)r.RawValue));
        }

        public void IfPropertyMatches<T>(Func<PropertyInfo, bool> matches, Func<GetStringRequest, T, string> display)
        {
            IfPropertyMatches(p => p.PropertyType.CanBeCastTo<T>() && matches(p), r => display(r, (T)r.RawValue));
        }
    }

    public class GetStringRequest
    {
        public GetStringRequest(){}

        public GetStringRequest(object model, PropertyInfo property, object rawValue)
        {
            Model = model;
            Property = property;
            RawValue = rawValue;
        }

        public object Model { get; set; }
        public PropertyInfo Property { get; set; }
        public object RawValue { get; set; }
    }
}