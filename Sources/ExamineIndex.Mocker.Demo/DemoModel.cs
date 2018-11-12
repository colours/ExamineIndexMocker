namespace ExamineIndex.Mocker.Demo
{
    using Exposed.Implementation;
    using Exposed.Interfaces;

    public class DemoModel : IExamineDocument
    {
        // No need to mark this field with ExamineFieldAttribute - it will appear in the index as Id field automatically.
        // However, you can place the ExamineFieldAttribute on it if a different name for Id field is needed (or type).
        public int Id { get; set; }

        // The field with the same name as the name of the property will be created in the index. The casing matters.
        [ExamineField]
        public string Text { get; set; }

        // The field with the name as specified by the FieldName property of the attribute will be created in the index with the type specified by ExamineFieldType property.
        [ExamineField(FieldName = "count", ExamineFieldType = ExamineFieldType.Int)]
        public int? Count { get; set; }
    }
}