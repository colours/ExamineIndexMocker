namespace ExamineIndex.Mocker.Demo
{
    using System;
    using System.Diagnostics;
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
    public class DemoTests
    {
        [Test]
        public void TestFindById()
        {
            var model = new[]
            {
                new DemoModel {Id = 1},
                new DemoModel {Id = 2},
                new DemoModel {Id = 3}
            };

            var index = Context.GetExamineIndexFactory().Get(model, new StandardAnalyzer(Version.LUCENE_29));

            var searcher = index.LuceneSearcherProvider;

            var sc = searcher.CreateSearchCriteria();
            var query = sc.Field("__NodeId", "2");
            var result = searcher.Search(query.Compile());

            Assert.That(result != null && result.TotalItemCount == 1 && result.First().Id == 2);
        }

        [Test]
        public void TestFindByCustomField()
        {
            var model = new[]
            {
                new DemoModel {Id = 1, Text = "First"},
                new DemoModel {Id = 2, Text = "Second"},
                new DemoModel {Id = 3, Text = "Third"}
            };

            var index = Context.GetExamineIndexFactory().Get(model, new StandardAnalyzer(Version.LUCENE_29));

            var searcher = index.LuceneSearcherProvider;

            const string textFieldName = "Text";

            var sc = searcher.CreateSearchCriteria();
            var query = sc.Field(textFieldName, "Third");
            var result = searcher.Search(query.Compile());

            Assert.That(result != null && result.TotalItemCount == 1 && result.First().Id == 3 &&
                        result.First().Fields.ContainsKey(textFieldName) && result.First().Fields[textFieldName] == "Third");
        }

        [Test]
        public void TestFindByDynamicallyAddedField()
        {
            var model = new[]
            {
                new DemoModel {Id = 1},
                new DemoModel {Id = 2},
                new DemoModel {Id = 3}
            };

            const string newFieldName = "New_Text_Field";

            var standardAnalyzer = new StandardAnalyzer(Version.LUCENE_29);

            var index = Context.GetExamineIndexFactory()
                .Get(model, standardAnalyzer,
                    gatheringNodeDataHandler: (sender, args) => args.Fields.Add(newFieldName, args.NodeId + " document"));

            var searcher = index.LuceneSearcherProvider;

            var sc = searcher.CreateSearchCriteria();
            var query = sc.Field(newFieldName, "2 document");
            var result = searcher.Search(query.Compile());

            Assert.That(result != null && result.TotalItemCount == 1 &&
                        result.First().Fields.ContainsKey(newFieldName) &&
                        result.First().Fields[newFieldName] == "2 document");
        }

        [Test]
        public void TestFindByAllFields()
        {
            var model = new[]
            {
                new DemoModel {Id = 1, Text = "First", Count = 11},
                new DemoModel {Id = 2, Text = "Second"},
                new DemoModel {Id = 3, Text = "Third", Count = 33}
            };

            const string newDoubleFieldName = "New_Double_Field";

            EventHandler<DocumentWritingEventArgs> documentWritingHandler = (sender, args) =>
            {
                double newDoubleFieldValue = args.NodeId * 0.2d;

                var newNumericField = new NumericField(newDoubleFieldName, Field.Store.NO, true);

                newNumericField.SetDoubleValue(Math.Round(newDoubleFieldValue, 1));

                args.Document.Add(newNumericField);
            };

            string indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "indexes", new StackTrace().GetFrame(0).GetMethod().Name, DateTime.Now.ToString("yyyyMMddhhmmss"));

            var standardAnalyzer = new StandardAnalyzer(Version.LUCENE_29);

            var index = Context.GetExamineIndexFactory()
                .Get(model, standardAnalyzer,
                    documentWritingHandler: documentWritingHandler,
                    indexDirectoryPath: indexPath);

            var searcher = index.LuceneSearcherProvider.GetSearcher();

            //searching using lucene
            BooleanQuery queryContainer = new BooleanQuery();

            var query = new QueryParser(Version.LUCENE_29, "Text", standardAnalyzer);
            Query textQuery = query.Parse("Third");

            Query idQuery = new TermQuery(new Term("__NodeId", "3"));
            Query newDoubleFieldQuery = new TermQuery(new Term(newDoubleFieldName, NumericUtils.DoubleToPrefixCoded(0.6d)));
            Query countQuery = new TermQuery(new Term("count", NumericUtils.IntToPrefixCoded(33)));

            queryContainer.Add(new BooleanClause(idQuery, BooleanClause.Occur.MUST));

            queryContainer.Add(new BooleanClause(newDoubleFieldQuery, BooleanClause.Occur.MUST));
            queryContainer.Add(new BooleanClause(textQuery, BooleanClause.Occur.MUST));
            queryContainer.Add(new BooleanClause(countQuery, BooleanClause.Occur.MUST));

            var result = searcher.Search(queryContainer, null, searcher.MaxDoc());
            var resultCollection = result.ScoreDocs.Select(p => searcher.Doc(p.doc)).ToList();

            Assert.That(resultCollection.Count == 1
                && resultCollection.First().GetField("__NodeId").StringValue() == "3"
                && resultCollection.First().GetField(newDoubleFieldName) == null//this param should not be in the return result
                && resultCollection.First().GetField("Text").StringValue() == "Third"
                && int.Parse(resultCollection.First().GetField("count").StringValue()) == 33);
        }
    }
}
