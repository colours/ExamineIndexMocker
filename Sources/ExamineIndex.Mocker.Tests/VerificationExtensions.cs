namespace ExamineIndex.Mocker.Tests
{
    using System.Linq;
    using Exposed.Interfaces;
    using Lucene.Net.Util;
    using NUnit.Framework;

    public static class VerificationExtensions
    {
        public static void VerifyFieldValue(this IExamineIndex index, int id, string fieldName, string fieldValue)
        {
            var searcher = index.LuceneSearcherProvider;

            var sc = searcher.CreateSearchCriteria();
            var query = sc.Field(fieldName, fieldValue);
            var result = searcher.Search(query.Compile());

            Assert.NotNull(result, "The result colletion is null when searching for '{0}' field.", fieldName);
            Assert.IsNotEmpty(result, "The result collection is empty when searching for '{0}' field.", fieldName);
            Assert.True(result.FirstOrDefault(p => p.Id == id) != null,
                "The result collection has no document with ID '{0}'.", id);
            Assert.True(result.First().Fields.ContainsKey(fieldName),
                "The field '{0}' does not exist in the document with ID '{1}'", fieldName, id);
            Assert.True(result.First().Fields[fieldName] == fieldValue,
                "The field '{0}' in the document with ID '{1}' does not contain the expected value '{2}'. The actual value is '{3}'.",
                fieldName, id, fieldValue, result.First().Fields[fieldName]);
        }

        public static void VerifyFieldValue(this IExamineIndex index, int id, string fieldName, int fieldValue)
        {
            var searcher = index.LuceneSearcherProvider;

            var convertedFieldValue = NumericUtils.IntToPrefixCoded(fieldValue);
            var sc = searcher.CreateSearchCriteria();
            var query = sc.Field(fieldName, convertedFieldValue);
            var result = searcher.Search(query.Compile());

            Assert.NotNull(result, "The result colletion is null when searching for '{0}' field.", fieldName);
            Assert.IsNotEmpty(result, "The result collection is empty when searching for '{0}' field.", fieldName);
            Assert.True(result.FirstOrDefault(p => p.Id == id) != null,
                "The result collection has no document with ID '{0}'.", id);
            Assert.True(result.First().Fields.ContainsKey(fieldName),
                "The field '{0}' does not exist in the document with ID '{1}'", fieldName, id);
            Assert.True(result.First().Fields[fieldName] == convertedFieldValue,
                "The field '{0}' in the document with ID '{1}' does not contain the expected value '{2}'. The actual value is '{3}'.",
                fieldName, id, fieldValue, result.First().Fields[fieldName]);
        }
    }
}