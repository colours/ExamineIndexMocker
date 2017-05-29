namespace ExamineIndex.Mocker.Internal.Factories
{
    using Exposed.Implementation;

    internal static class ExamineFieldTypeFactory
    {
        internal static string Get(ExamineFieldType examineFieldTypeEnum)
        {
            string examineFieldType = null;

            switch (examineFieldTypeEnum)
            {
                case ExamineFieldType.DateTime:
                    examineFieldType = Constants.ExamineFieldTypes.DateTime;
                    break;
                case ExamineFieldType.Year:
                    examineFieldType = Constants.ExamineFieldTypes.Year;
                    break;
                case ExamineFieldType.Month:
                    examineFieldType = Constants.ExamineFieldTypes.Month;
                    break;
                case ExamineFieldType.Day:
                    examineFieldType = Constants.ExamineFieldTypes.Day;
                    break;
                case ExamineFieldType.Hour:
                    examineFieldType = Constants.ExamineFieldTypes.Hour;
                    break;
                case ExamineFieldType.Minute:
                    examineFieldType = Constants.ExamineFieldTypes.Minute;
                    break;
                case ExamineFieldType.Double:
                    examineFieldType = Constants.ExamineFieldTypes.Double;
                    break;
                case ExamineFieldType.Int:
                    examineFieldType = Constants.ExamineFieldTypes.Int;
                    break;
                case ExamineFieldType.Long:
                    examineFieldType = Constants.ExamineFieldTypes.Long;
                    break;
                case ExamineFieldType.Number:
                    examineFieldType = Constants.ExamineFieldTypes.Number;
                    break;
            }

            return examineFieldType;
        }
    }
}
