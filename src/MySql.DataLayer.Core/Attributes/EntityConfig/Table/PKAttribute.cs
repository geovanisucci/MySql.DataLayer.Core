using System;

namespace MySql.DataLayer.Core.Attributes.EntityConfig.Table
{
    [System.AttributeUsage(AttributeTargets.Property)]
    public class PKAttribute : Attribute
    {
         /// <summary>
        /// The actual name of the column that is in the table in the database
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }

        public PKAttribute(string pkColumnName)
        {
            Name = pkColumnName;
        }
    }
}