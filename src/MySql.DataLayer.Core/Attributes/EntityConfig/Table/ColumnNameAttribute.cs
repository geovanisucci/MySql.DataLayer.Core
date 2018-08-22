namespace MySql.DataLayer.Core.Attributes.EntityConfig.Table
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// The actual name of the column that is in the table in the database
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }

        public ColumnNameAttribute(string columnName)
        {
            Name = columnName;
        }
    }
}