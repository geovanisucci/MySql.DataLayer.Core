namespace MySql.DataLayer.Core.Attributes.StoredProcedureConfig.StoredProcedure
{
     using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class StoredProcedureNameAttribute : Attribute
    {
        /// <summary>
        /// The actual name the StoredProcedure in database.
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }

        public StoredProcedureNameAttribute(string storedProcedureName)
        {
            Name = storedProcedureName;
        }
    }
}