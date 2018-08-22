namespace MySql.DataLayer.Core.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System;
    using MySql.DataLayer.Core.Attributes.EntityConfig.Table;

    public class Utilities
    {
        private static Dictionary<string, string> _columnNames = new Dictionary<string, string>();
        private static Dictionary<string, string> _pkColumnName = new Dictionary<string, string>();
        private static Dictionary<Type, string> _tableNames = new Dictionary<Type, string>();
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
                name = $"`{name}`";
                _tableNames.Add(type, name);
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


    }
}