using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;


namespace Common.ExtensionMethods
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
            return source;
        }

        public static bool IsCollectionValid<T>(this ICollection<T> source)
        {
            return source != null && source.Count > 0;
        }

        public static bool IsCollectionValid<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static IEnumerable<T> Distinct<T, TK>(this IEnumerable<T> source, Func<T, TK> keySelector)
        {
            var dict = new HashSet<TK>();

            foreach (var item in source)
            {
                var key = keySelector(item);

                if (!dict.Contains(key))
                {
                    dict.Add(key);
                    yield return item;
                }
            }
        }

        public static List<T> ToListOrDefault<T>(this IEnumerable<T> source)
        {
            return (source != null && source.Any()) ? source.ToList() : null;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string[] colHeadersExclude = null)
        {
            if (source != null && source.Any())
            {
                var entityType = typeof(T);

                var propertyHeaderCollection = TypeDescriptor.GetProperties(entityType);

                var dataTable = CreateDataTable(propertyHeaderCollection, entityType.Name, colHeadersExclude);

                FillDataTable(dataTable, propertyHeaderCollection, source, colHeadersExclude);

                return dataTable;
            }
            return null;
        }

        private static void FillDataTable<T>(DataTable dataTable, IEnumerable propertyHeaderCollection, IEnumerable<T> stagingDataColl, string[] colHeadersExclude = null)
        {
            var isExcludeHeadersPrsesent = colHeadersExclude.IsCollectionValid();
            foreach (T dataItem in stagingDataColl)
            {
                var dataRow = dataTable.NewRow();
                foreach (PropertyDescriptor propertyInfo in propertyHeaderCollection)
                {
                    var colHeaderName = propertyInfo.Name;
                    if (isExcludeHeadersPrsesent)
                    {
                        if (!colHeadersExclude.Contains(colHeaderName))
                        {
                            dataRow[colHeaderName] = propertyInfo.GetValue(dataItem) ?? DBNull.Value;
                        }
                    }
                    else
                    {
                        dataRow[colHeaderName] = propertyInfo.GetValue(dataItem) ?? DBNull.Value;
                    }

                }
                dataTable.Rows.Add(dataRow);
            }

        }

        private static DataTable CreateDataTable(PropertyDescriptorCollection propertyHeaderCollection, string dataTableName, string[] colHeadersExclude = null)
        {
            var isExcludeHeadersPrsesent = colHeadersExclude.IsCollectionValid();
            var dataTable = new DataTable(dataTableName);
            foreach (PropertyDescriptor propertyInfo in propertyHeaderCollection)
            {
                var colHeaderName = propertyInfo.Name;
                if (!isExcludeHeadersPrsesent || !colHeadersExclude.Contains(colHeaderName))
                {
                    var propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsNullable())
                        propertyType = Nullable.GetUnderlyingType(propertyType);

                    dataTable.Columns.Add(colHeaderName, propertyType);
                }
            }
            return dataTable;
        }
    }
}
