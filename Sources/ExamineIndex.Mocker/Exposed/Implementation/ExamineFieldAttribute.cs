namespace ExamineIndex.Mocker.Exposed.Implementation
{
    using System;

    /// <summary>
    /// Defines the field name, type and sorting possibility. 
    /// Use this attribute for the properties you want to be indexed.
    /// </summary>
    public class ExamineFieldAttribute : Attribute
    {
        /// <summary>
        /// The name of the field to be used during indexing.
        /// </summary>
        public string FieldName;

        /// <summary>
        /// Optional. The examine type of the field. 
        /// Defines if the field value will be indexed as a plain text or a binary data.
        /// </summary>
        public ExamineFieldType ExamineFieldType;

        /// <summary>
        /// Optional. Defines if sorting is enabled for the field.
        /// </summary>
        public bool EnableSorting;
    }
}
