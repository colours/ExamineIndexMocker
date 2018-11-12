namespace ExamineIndex.Mocker.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Examine.LuceneEngine;
    using Exposed.Implementation;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Util;
    using NUnit.Framework;
    using Version = Lucene.Net.Util.Version;

    [TestFixture]
    public class IndexTests
    {
        [Test]
        public void TestIdFieldsAppearInTheIndex()
        {
            var model = new[]
            {
                new TestModel {Id = 1},
                new TestModel {Id = 22},
                new TestModel {Id = 35}
            };

            var index = Context.GetExamineIndexFactory().Get(model, new StandardAnalyzer(Version.LUCENE_29));

            // test if default _NodeId field is present in the index.
            index.VerifyFieldValue(35, "__NodeId", "35");
            // test if Id field with custom name is present in the index.
            index.VerifyFieldValue(22, "CustomId", "22");
        }

        [Test]
        public void TestMultipleCustomFieldsAppearInTheIndex()
        {
            var model = new[]
            {
                new TestModel {Id = 1, Text = "First"},
                new TestModel {Id = 2, Text = "Second"},
                new TestModel {Id = 3, Text = "Third text."}
            };

            var index = Context.GetExamineIndexFactory().Get(model, new StandardAnalyzer(Version.LUCENE_29));

            index.VerifyFieldValue(3, "Text", "Third text.");
            index.VerifyFieldValue(3, "CustomText", "Third text.");
            index.VerifyFieldValue(3, "CustomText2", "Third text.");
        }

        [Test]
        public void TestCustomFieldTypesAppearInTheIndex()
        {
            var currentDateTime = DateTime.Now;

            var model = new[]
            {
                new TestModel {Id = 1, NumberField = short.MaxValue},
                new TestModel {Id = 2, IntField = int.MaxValue},
                new TestModel {Id = 3, FloatField = 342.577f},
                new TestModel {Id = 4, DoubleField = 12.21d},
                new TestModel {Id = 5, LongField = long.MaxValue},
                new TestModel {Id = 6, DateTimeField = currentDateTime},
                new TestModel {Id = 7, YearField = currentDateTime},
                new TestModel {Id = 8, MonthField = currentDateTime},
                new TestModel {Id = 9, DayField = currentDateTime},
                new TestModel {Id = 10, HourField = currentDateTime},
                new TestModel {Id = 11, MinuteField = currentDateTime},
                new TestModel {Id = 12, StringField = "Test Text"}
            };

            var index = Context.GetExamineIndexFactory().Get(model, new StandardAnalyzer(Version.LUCENE_29));

            //index.VerifyFieldValue(1, "NumberField", short.MaxValue.ToString());
            index.VerifyFieldValue(2, "IntField", int.MaxValue);
            index.VerifyFieldValue(3, "FloatField", 342.577f.ToString(CultureInfo.InvariantCulture));
            index.VerifyFieldValue(4, "DoubleField", 12.21d.ToString(CultureInfo.InvariantCulture));
            index.VerifyFieldValue(5, "LongField", long.MaxValue.ToString());
            index.VerifyFieldValue(6, "DateTimeField", currentDateTime.ToString(CultureInfo.InvariantCulture));
            index.VerifyFieldValue(7, "YearField", currentDateTime.Year.ToString());
            index.VerifyFieldValue(8, "MonthField", currentDateTime.Month.ToString());
            index.VerifyFieldValue(9, "DayField", currentDateTime.Day.ToString());
            index.VerifyFieldValue(10, "HourField", currentDateTime.Hour.ToString());
            index.VerifyFieldValue(11, "MinuteField", currentDateTime.Minute.ToString());
            index.VerifyFieldValue(12, "StringField", "Test Text");
        }

        //[Test]
        //public void TestFindByDynamicallyAddedField()
        //{
        //    var model = new[]
        //    {
        //        new TestModel {Id = 1},
        //        new TestModel {Id = 2},
        //        new TestModel {Id = 3}
        //    };

        //    const string newFieldName = "New_Text_Field";

        //    var standardAnalyzer = new StandardAnalyzer(Version.LUCENE_29);

        //    var index = Context.GetExamineIndexFactory()
        //        .Get(model, standardAnalyzer,
        //            gatheringNodeDataHandler: (sender, args) => args.Fields.Add(newFieldName, args.NodeId + " document"));

        //    var searcher = index.LuceneSearcherProvider;

        //    var sc = searcher.CreateSearchCriteria();
        //    var query = sc.Field(newFieldName, "2 document");
        //    var result = searcher.Search(query.Compile());

        //    Assert.That(result != null && result.TotalItemCount == 1 &&
        //                result.First().Fields.ContainsKey(newFieldName) &&
        //                result.First().Fields[newFieldName] == "2 document");
        //}

        //[Test]
        //public void TestFindByAllFields()
        //{
        //    var model = new[]
        //    {
        //        new TestModel {Id = 1, Text = "First", Count = 11},
        //        new TestModel {Id = 2, Text = "Second"},
        //        new TestModel {Id = 3, Text = "Third", Count = 33}
        //    };

        //    const string newDoubleFieldName = "New_Double_Field";

        //    EventHandler<DocumentWritingEventArgs> documentWritingHandler = (sender, args) =>
        //    {
        //        double newDoubleFieldValue = args.NodeId * 0.2d;

        //        var newNumericField = new NumericField(newDoubleFieldName, Field.Store.NO, true);

        //        newNumericField.SetDoubleValue(Math.Round(newDoubleFieldValue, 1));

        //        args.Document.Add(newNumericField);
        //    };

        //    string indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "indexes", new StackTrace().GetFrame(0).GetMethod().Name, DateTime.Now.ToString("yyyyMMddhhmmss"));

        //    var standardAnalyzer = new StandardAnalyzer(Version.LUCENE_29);

        //    var index = Context.GetExamineIndexFactory()
        //        .Get(model, standardAnalyzer,
        //            documentWritingHandler: documentWritingHandler,
        //            indexDirectoryPath: indexPath);

        //    var searcher = index.LuceneSearcherProvider.GetSearcher();

        //    //searching using lucene
        //    BooleanQuery queryContainer = new BooleanQuery();

        //    var query = new QueryParser(Version.LUCENE_29, "Text", standardAnalyzer);
        //    Query textQuery = query.Parse("Third");

        //    Query idQuery = new TermQuery(new Term("__NodeId", "3"));
        //    Query newDoubleFieldQuery = new TermQuery(new Term(newDoubleFieldName, NumericUtils.DoubleToPrefixCoded(0.6d)));
        //    Query countQuery = new TermQuery(new Term("count", NumericUtils.IntToPrefixCoded(33)));

        //    queryContainer.Add(new BooleanClause(idQuery, BooleanClause.Occur.MUST));

        //    queryContainer.Add(new BooleanClause(newDoubleFieldQuery, BooleanClause.Occur.MUST));
        //    queryContainer.Add(new BooleanClause(textQuery, BooleanClause.Occur.MUST));
        //    queryContainer.Add(new BooleanClause(countQuery, BooleanClause.Occur.MUST));

        //    var result = searcher.Search(queryContainer, null, searcher.MaxDoc());
        //    var resultCollection = result.ScoreDocs.Select(p => searcher.Doc(p.doc)).ToList();

        //    Assert.That(resultCollection.Count == 1
        //        && resultCollection.First().GetField("__NodeId").StringValue() == "3"
        //        && resultCollection.First().GetField(newDoubleFieldName) == null//this param should not be in the return result
        //        && resultCollection.First().GetField("Text").StringValue() == "Third"
        //        && int.Parse(resultCollection.First().GetField("count").StringValue()) == 33);
        //}
    }
}
