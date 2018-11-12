namespace ExamineIndex.Mocker.Tests
{
    using System;
    using Exposed.Implementation;
    using Exposed.Interfaces;

    public class TestModel : IExamineDocument
    {
        [ExamineField(FieldName = "CustomId")]
        public int Id { get; set; }

        [ExamineField]
        [ExamineField(FieldName = "CustomText")]
        [ExamineField(FieldName = "CustomText2")]
        public string Text { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Number)]
        public float NumberField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Int)]
        public int IntField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Float)]
        public double FloatField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Double)]
        public double DoubleField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Long)]
        public long LongField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.String)]
        public string StringField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.DateTime)]
        public DateTime DateTimeField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Year)]
        public DateTime YearField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Month)]
        public DateTime MonthField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Day)]
        public DateTime DayField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Hour)]
        public DateTime HourField { get; set; }

        [ExamineField(ExamineFieldType = ExamineFieldType.Minute)]
        public DateTime MinuteField { get; set; }
    }
}