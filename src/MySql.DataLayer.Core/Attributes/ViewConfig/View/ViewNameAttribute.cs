namespace MySql.DataLayer.Core.Attributes.ViewConfig.View
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ViewNameAttribute  : Attribute
    {
        public string Name { get; private set; }

        public ViewNameAttribute(string viewName)
        {
            Name = viewName;
        }
    }
}