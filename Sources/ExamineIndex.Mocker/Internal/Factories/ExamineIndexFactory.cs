namespace ExamineIndex.Mocker.Internal.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Examine;
    using Examine.LuceneEngine;
    using Examine.LuceneEngine.Providers;

    using Exposed.Interfaces;
    using Mocks;

    using Lucene.Net.Analysis;
    using Lucene.Net.Store;

    using Rhino.Mocks;

    internal class ExamineIndexFactory : IExamineIndexFactory
    {
        public IExamineIndex Get<T>(
            IEnumerable<T> source,
            Analyzer analyzer,
            IEnumerable<string> includeDocumentTypes = null,
            IEnumerable<string> excludeDocumentTypes = null,
            EventHandler<DocumentWritingEventArgs> documentWritingHandler = null,
            EventHandler<IndexingNodeDataEventArgs> gatheringNodeDataHandler = null,
            string indexDirectoryPath = null) where T : class, IExamineDocument
        {

            Directory dir;

            if (!string.IsNullOrEmpty(indexDirectoryPath))
            {
                dir = FSDirectory.Open(System.IO.Directory.CreateDirectory(indexDirectoryPath));
            }
            else
            {
                dir = new RAMDirectory();
            }

            var userFieldSets = ExamineUserFieldSetFactory.Get(typeof(T));

            var examineIndex = new ExamineIndex
            {
                UserFields = userFieldSets,
                IncludeNodeTypes = (includeDocumentTypes ?? new string[] { }).ToArray(),
                ExcludeNodeTypes = (excludeDocumentTypes ?? new string[] { }).ToArray(),
                SimpleDataService = MockRepository.GenerateMock<ISimpleDataService>(),
                LuceneDir = dir
            };

            examineIndex.IndexCriteria = new IndexCriteria(
                Enumerable.Empty<IIndexField>(),
                userFieldSets,
                examineIndex.IncludeNodeTypes,
                examineIndex.ExcludeNodeTypes,
                -1);

            examineIndex.Analyzer = analyzer;

            examineIndex.Indexer = new SimpleDataIndexer(
                examineIndex.IndexCriteria,
                examineIndex.LuceneDir,
                examineIndex.Analyzer,
                examineIndex.SimpleDataService,
                new[] { Constants.IndexType },
                false);

            if (documentWritingHandler != null)
            {
                ((LuceneIndexer)examineIndex.Indexer).DocumentWriting += documentWritingHandler;
            }

            if (gatheringNodeDataHandler != null)
            {
                ((LuceneIndexer)examineIndex.Indexer).GatheringNodeData += gatheringNodeDataHandler;
            }

            examineIndex.LuceneSearcherProvider = new LuceneSearcher(dir, analyzer);

            var dataSet =
                source.Select(
                    (itemToIndex, index) =>
                        ExamineSimpleDataSetFactory.Get(itemToIndex))
                .Where(p => p != null);

            var examineSimpleDataSet = new ExamineSimpleDataSet(Constants.IndexType, dataSet);

            examineIndex.SimpleDataService.Stub(p => p.GetAllData(Constants.IndexType)).Return(examineSimpleDataSet);

            examineIndex.Indexer.RebuildIndex();

            return examineIndex;
        }
    }
}
