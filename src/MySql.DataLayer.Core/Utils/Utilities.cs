namespace MySql.DataLayer.Core.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System;
    using MySql.DataLayer.Core.Attributes.EntityConfig.Table;
    using MySql.DataLayer.Core.Attributes.StoredProcedureConfig.StoredProcedure;
    using System.Data;

    public class Utilities
    {
        private static Dictionary<string, string> _columnNames = new Dictionary<string, string>();
        private static Dictionary<string, string> _pkColumnName = new Dictionary<string, string>();
        private static Dictionary<Type, string> _tableNames = new Dictionary<Type, string>();
        private static Dictionary<Type, string> _storedProcedureNames = new Dictionary<Type, string>();
        private static Dictionary<Type, DbType> _typeMap;

        public static string ConditionToString(Condition condition)
        {
            switch (condition)
            {
                case Condition.Where: return " Where ";
                case Condition.And: return " And ";
                case Condition.Or: return " Or ";

            }
            return "";
        }

        public static string OperatorToString(ConditionOperator conditionOperator)
        {
            switch (conditionOperator)
            {
                case ConditionOperator.Diff: return " <> ";
                case ConditionOperator.Equal: return " = ";
                case ConditionOperator.Gt: return " > ";
                case ConditionOperator.Gte: return " >= ";
                case ConditionOperator.Lt: return " < ";
                case ConditionOperator.Lte: return " <= ";
                case ConditionOperator.Isnull: return " Is Null ";
                case ConditionOperator.NotNull: return " Is Not Null";

            }
            return "";
        }


        public static string GetColumnName(Type type, PropertyInfo property, bool sqlFormat = true)
        {
            string columnKey = $"{type}:{property.Name}:{sqlFormat}";
            string name = "";
            _columnNames.TryGetValue(columnKey, out name);
            if (name == null)
            {
                var attribute = property.GetCustomAttribute<ColumnNameAttribute>();
                if (attribute == null)
                {
                    name = property.Name;
                }
                else
                {
                    name = attribute.Name;
                }
                if (sqlFormat)
                {
                    string tableName = GetTableName(type);
                    name = $"{tableName}.`{name}`";
                }
                else
                {
                    name = $"{name}";
                }
                _columnNames.Add(columnKey, name);
            }
            return name;
        }
        public static string GetColumnName<T>(PropertyInfo property) where T : IDataEntity
        {
            Type tableType = typeof(T);
            return GetColumnName(tableType, property);
        }
        public static string GetTableName<T>() where T : IDataEntity
        {
            Type type = typeof(T);
            return GetTableName(type);
        }

        public static string GetStoredProcedureName<T>() where T : IDataStoredProcedure
        {
            Type type = typeof(T);
            return GetStoredProcedureName(type);
        }

        public static string GetTableName(Type type)
        {
            string name = null;
            _tableNames.TryGetValue(type, out name);
            if (name == null)
            {
                var attribute = type.GetCustomAttribute<TableNameAttribute>();
                if (attribute == null)
                {
                    name = type.Name.Replace("Entity", "");
                    name = type.Name.Replace("Model", "");
                }
                else
                {
                    name = attribute.Name;
                }
                //name = $"`{name}`";
                _tableNames.Add(type, name);
            }
            return name;
        }

        public static string GetStoredProcedureName(Type type)
        {
            string name = null;
            _storedProcedureNames.TryGetValue(type, out name);
            if (name == null)
            {
                var attribute = type.GetCustomAttribute<StoredProcedureNameAttribute>();
                if (attribute == null)
                {
                    name = type.Name;
                }
                else
                {
                    name = attribute.Name;
                }

                name = $"`{name}`";
                _storedProcedureNames.Add(type, name);
            }
            return name;
        }

        public static string GetPkColumnName(PropertyInfo property, bool sqlFormat = true)
        {
            string name = null;

            if (IsKeyProperty(property))
            {
                var attribute = property.GetCustomAttribute<PKAttribute>();
                if (attribute == null)
                {
                    name = property.Name;
                }
                else
                {
                    name = attribute.Name;
                }
                if (sqlFormat)
                {
                    name = $"`{name}`";
                }
                else
                {
                    name = $"{name}";
                }
            }
            return name;
        }

        public static string GetPkColumnName<T>(bool sqlFormat = true) where T : IDataEntity
        {
            string pkName = "";
            var properties = GetProperties<T>();
            foreach (PropertyInfo p in properties)
            {
                pkName = GetPkColumnName(p, sqlFormat);

                if (!string.IsNullOrEmpty(pkName))
                    break;
            }
            return pkName;
        }


        public static List<PropertyInfo> GetProperties(Type type)
        {
            List<PropertyInfo> properties = null;


            if (properties == null)
            {
                properties = type.GetProperties().ToList();

            }

            return properties;
        }
        public static List<PropertyInfo> GetProperties<T>()
         => GetProperties(typeof(T));


        public static bool IsKeyProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(false).ToList().
                        Where(x => x.GetType().Name == "PKAttribute").FirstOrDefault() != null;
        }


        public static DbType PropertyToDatabaseType(object property, Type typeOfNullProperty = null)
        {
            if (_typeMap == null)
                FillDatabaseTypeMapping();

            DbType result;

            Type propertyType;

            //If the property has value get type of this property by reflection
            if (property != null)
                propertyType = property.GetType();
            //If property value is null need to pass the type as a parameter because isn't possible to get by reflection
            else if (property == null && typeOfNullProperty != null)
            {
                //If type isn't Nullable<T> transform in Nullable<T>
                if (typeOfNullProperty.IsValueType)
                    propertyType =  typeof(Nullable<>).MakeGenericType(typeOfNullProperty);
                else
                    propertyType = typeOfNullProperty;
            }             
            //If property and typeOfNullProperty is null it's impossible to define the type
            else
                throw new Exception("Can't define the type");

            var hasValue = _typeMap.TryGetValue(propertyType, out result);

            if (hasValue)
                return result;
            else
                throw new Exception("Conversion error! DatabaseType Not found");
        }

        private static void FillDatabaseTypeMapping()
        {
            _typeMap = new Dictionary<Type, DbType>();

            _typeMap.Add(typeof(byte), DbType.Byte);
            _typeMap.Add(typeof(sbyte), DbType.SByte);
            _typeMap.Add(typeof(short), DbType.Int16);
            _typeMap.Add(typeof(ushort), DbType.UInt16);
            _typeMap.Add(typeof(int), DbType.Int32);
            _typeMap.Add(typeof(uint), DbType.UInt32);
            _typeMap.Add(typeof(long), DbType.Int64);
            _typeMap.Add(typeof(ulong), DbType.UInt64);
            _typeMap.Add(typeof(float), DbType.Single);
            _typeMap.Add(typeof(double), DbType.Double);
            _typeMap.Add(typeof(decimal), DbType.Decimal);
            _typeMap.Add(typeof(bool), DbType.Boolean);
            _typeMap.Add(typeof(string), DbType.String);
            _typeMap.Add(typeof(char), DbType.StringFixedLength);
            _typeMap.Add(typeof(Guid), DbType.Guid);
            _typeMap.Add(typeof(DateTime), DbType.DateTime);
            _typeMap.Add(typeof(DateTimeOffset), DbType.DateTimeOffset);
            _typeMap.Add(typeof(byte[]), DbType.Binary);
            _typeMap.Add(typeof(byte?), DbType.Byte);
            _typeMap.Add(typeof(sbyte?), DbType.SByte);
            _typeMap.Add(typeof(short?), DbType.Int16);
            _typeMap.Add(typeof(ushort?), DbType.UInt16);
            _typeMap.Add(typeof(int?), DbType.Int32);
            _typeMap.Add(typeof(uint?), DbType.UInt32);
            _typeMap.Add(typeof(long?), DbType.Int64);
            _typeMap.Add(typeof(ulong?), DbType.UInt64);
            _typeMap.Add(typeof(float?), DbType.Single);
            _typeMap.Add(typeof(double?), DbType.Double);
            _typeMap.Add(typeof(decimal?), DbType.Decimal);
            _typeMap.Add(typeof(bool?), DbType.Boolean);
            _typeMap.Add(typeof(char?), DbType.StringFixedLength);
            _typeMap.Add(typeof(Guid?), DbType.Guid);
            _typeMap.Add(typeof(DateTime?), DbType.DateTime);
            _typeMap.Add(typeof(DateTimeOffset?), DbType.DateTimeOffset);
        }
    }
}