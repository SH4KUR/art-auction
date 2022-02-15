using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using Xunit;

namespace ArtAuction.Tests.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class InlineMockAutoDataAttribute : CompositeDataAttribute
    {
        public IEnumerable<object> Values { get; }
        public AutoDataAttribute AutoDataAttribute { get; }

        public InlineMockAutoDataAttribute(params object[] values)
            : this(new MockAutoDataAttribute(), values)
        {
        }

        public InlineMockAutoDataAttribute(MockAutoDataAttribute autoDataAttribute, params object[] values)
            : base(new InlineDataAttribute(values), autoDataAttribute)
        {
            AutoDataAttribute = autoDataAttribute;
            Values = values;
        }
    }
}
