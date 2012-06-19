using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LogImporter.Database
{
    /// <summary>
    /// Sql Mapper Extensions: Dapper.Contrib (see https://github.com/SamSaffron/dapper-dot-net/blob/master/Dapper.Contrib/SqlMapperExtensions.cs)
    /// </summary>
    internal static class SqlMapperExtensions
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> typeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> typeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        public static string GetInsertStatement(Type type, string tableName = null)
        {
            var name = tableName ?? GetTableName(type);

            var sbColumnList = new StringBuilder(null);
            var sbParameterList = new StringBuilder(null);

            var allProperties = TypePropertiesCache(type);

            for (var i = 0; i < allProperties.Count(); i++)
            {
                var property = allProperties.ElementAt(i);

                // Column name
                sbColumnList.Append(property.Name);

                if (i < allProperties.Count() - 1)
                    sbColumnList.Append(", ");

                // Parameter name
                sbParameterList.AppendFormat("@{0}", property.Name);

                if (i < allProperties.Count() - 1)
                    sbParameterList.Append(", ");
            }

            return string.Format("insert into {0} ({1}) values ({2})", name, sbColumnList.ToString(), sbParameterList.ToString());
        }
        
        private static IEnumerable<PropertyInfo> TypePropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pis;

            if (typeProperties.TryGetValue(type.TypeHandle, out pis))
            {
                return pis;
            }

            var properties = type.GetProperties();

            typeProperties[type.TypeHandle] = properties;
            
            return properties;
        }

        private static string GetTableName(Type type)
        {
            string name;

            if (!typeTableName.TryGetValue(type.TypeHandle, out name))
            {
                name = type.Name + "s";

                if (type.IsInterface && name.StartsWith("I"))
                    name = name.Substring(1);

                // NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableattribute = type.GetCustomAttributes(false).Where(attr => attr.GetType().Name == "TableAttribute").SingleOrDefault() as dynamic;

                if (tableattribute != null)
                    name = tableattribute.Name;
                
                typeTableName[type.TypeHandle] = name;
            }

            return name;
        }
    }
}