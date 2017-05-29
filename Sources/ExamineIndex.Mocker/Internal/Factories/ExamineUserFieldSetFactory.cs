namespace ExamineIndex.Mocker.Internal.Factories
{
    using System;
    using System.Linq;

    using Exposed.Implementation;
    using Mocks;

    internal static class ExamineUserFieldSetFactory
    {
        internal static ExamineIndexFieldList Get(Type itemToIndexType)
        {
            ExamineIndexFieldList userFields = new ExamineIndexFieldList();

            var instanceAndInterfaceProperties =
                itemToIndexType.GetProperties()
                    .Concat(itemToIndexType.GetInterfaces().SelectMany(p => p.GetProperties()));

            foreach (var property in instanceAndInterfaceProperties)
            {
                var instanceAndInterfaceAttributes =
                    property.GetCustomAttributes(typeof(ExamineFieldAttribute), true).OfType<ExamineFieldAttribute>();

                foreach (var examineFieldTypeAttribute in instanceAndInterfaceAttributes)
                {
                    string examineFieldName = string.IsNullOrEmpty(examineFieldTypeAttribute.FieldName)
                                                  ? property.Name
                                                  : examineFieldTypeAttribute.FieldName;

                    if (!userFields.ContainsField(examineFieldName))
                    {
                        string examineFieldType = ExamineFieldTypeFactory.Get(
                            examineFieldTypeAttribute.ExamineFieldType);
                        bool enableSorting = examineFieldTypeAttribute.EnableSorting;

                        if (!string.IsNullOrEmpty(examineFieldName))
                        {
                            userFields.AddIndexField(examineFieldName, examineFieldType, enableSorting);
                        }
                    }
                }
            }

            return userFields;
        }
    }
}
