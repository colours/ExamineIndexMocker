namespace ExamineIndex.Mocker.Internal.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Examine;
    using Examine.LuceneEngine;

    using ExamineIndex.Mocker.Exposed.Implementation;
    using ExamineIndex.Mocker.Exposed.Interfaces;

    internal static class ExamineSimpleDataSetFactory
    {
        internal static SimpleDataSet Get<T>(T itemToIndex) where T : class, IExamineDocument
        {
            SimpleDataSet simpleDataSet = null;

            var rowData = GetRowData(itemToIndex);

            if (rowData != null)
            {
                simpleDataSet = new SimpleDataSet
                {
                    NodeDefinition = new IndexedNode
                    {
                        NodeId = itemToIndex.Id,
                        Type = Constants.IndexType
                    },
                    RowData = rowData
                };
            }

            return simpleDataSet;
        }

        private static Dictionary<string, string> GetRowData(object itemToIndex)
        {
            var rowData = new Dictionary<string, string>();

            var itemType = itemToIndex.GetType();

            var customAttributesOfInterfaceProperties =
                itemType.GetInterfaces()
                    .SelectMany(p => p.GetProperties())
                    .ToDictionary(
                        p => p.Name,
                        p => p.GetCustomAttributes(typeof(ExamineFieldAttribute), true).OfType<ExamineFieldAttribute>());

            foreach (var property in itemType.GetProperties().Where(p => p.CanRead))
            {
                object propertyValue = property.GetValue(itemToIndex, null);

                var instanceAttributes =
                    property.GetCustomAttributes(typeof(ExamineFieldAttribute), true).OfType<ExamineFieldAttribute>();

                var interfaceAttributes = customAttributesOfInterfaceProperties.ContainsKey(property.Name)
                                              ? customAttributesOfInterfaceProperties[property.Name]
                                              : Enumerable.Empty<ExamineFieldAttribute>();

                foreach (var examineFieldTypeAttribute in instanceAttributes.Concat(interfaceAttributes))
                {
                    string examineFieldName = string.IsNullOrEmpty(examineFieldTypeAttribute.FieldName)
                                                  ? property.Name
                                                  : examineFieldTypeAttribute.FieldName;

                    if (!rowData.ContainsKey(examineFieldName))
                    {
                        if (propertyValue != null)
                        {
                            string convertedValue = Convert.ToString(propertyValue, CultureInfo.InvariantCulture);
                            rowData.Add(examineFieldName, convertedValue);
                        }
                        else
                        {
                            rowData.Add(examineFieldName, null);
                        }
                    }
                }
            }

            return rowData;
        }
    }
}