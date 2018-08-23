namespace MySql.DataLayer.Core.Attributes.EntityConfig.Table
{
    using System;
     [AttributeUsage (AttributeTargets.Class, Inherited = false)]
    public class TableNameAttribute : Attribute 
    {
        /// <summary>
        /// The real table mapped name in database.
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }

        public TableNameAttribute(string tableName)
        {
            Name = tableName;
        }
    }
}