namespace ExamineIndex.Mocker.Internal.Mocks
{
    using System.Collections.Generic;

    using Examine;
    using Examine.LuceneEngine;
    using Examine.LuceneEngine.Providers;

    using Exposed.Interfaces;

    using Lucene.Net.Analysis;
    using Lucene.Net.Store;

    internal class ExamineIndex : IExamineIndex
    {
        public LuceneSearcher LuceneSearcherProvider { get; set; }

        internal IIndexer Indexer { get; set; }

        internal ISimpleDataService SimpleDataService { get; set; }

        internal Directory LuceneDir { get; set; }

        internal ExamineIndexFieldList StandardFields { get; set; }

        internal IIndexCriteria IndexCriteria { get; set; }

        internal Analyzer Analyzer { get; set; }

        internal ExamineIndexFieldList UserFields { get; set; }

        internal IEnumerable<string> IndexTypes { get; set; }

        internal IEnumerable<string> IncludeNodeTypes { get; set; }

        internal IEnumerable<string> ExcludeNodeTypes { get; set; }
    }
}