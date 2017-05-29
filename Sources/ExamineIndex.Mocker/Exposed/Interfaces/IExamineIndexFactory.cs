namespace ExamineIndex.Mocker.Exposed.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Examine;
    using Examine.LuceneEngine;

    using Lucene.Net.Analysis;

    /// <summary>
    /// Defines the contract for the examine index factory.
    /// </summary>
    public interface IExamineIndexFactory
    {
        /// <summary>
        /// Creates an instance of the examine index.
        /// </summary>
        /// <typeparam name="T">The type of the examine document DTO. The properties should be marked with ExamineFieldAttribute in order to be indexed. You can also mark interface properties with this attribute.</typeparam>
        /// <param name="source">The collection of the document DTOs to be indexed. The DTO properties should be marked with ExamineFieldAttribute in order to be indexed.</param>
        /// <param name="analyzer">The default analyzer to be used during indexing.</param>
        /// <param name="includeDocumentTypes">Optional. The collection of document types to be included during indexing.</param>
        /// <param name="excludeDocumentTypes">Optional. The collection of document types to be excluded during indexing.</param>
        /// <param name="documentWritingHandler">Optional. The event handler that is called when the lucene document is ready to be indexed. During this stage new fields can be added to the document or the existing ones removed.</param>
        /// <param name="gatheringNodeDataHandler">Optional. The event handler that is called when the examine has collected data about the fields to be indexed. During this stage new fields can be added or the existing ones removed.</param>
        /// <param name="indexDirectoryPath">Optional. The index directory path. If this field is not specified the index will be created in the RAM. Use this option to explore your index.</param>
        /// <returns>The examine (lucene) index ready for searching.</returns>
        IExamineIndex Get<T>(
            IEnumerable<T> source,
            Analyzer analyzer,
            IEnumerable<string> includeDocumentTypes = null,
            IEnumerable<string> excludeDocumentTypes = null,
            EventHandler<DocumentWritingEventArgs> documentWritingHandler = null,
            EventHandler<IndexingNodeDataEventArgs> gatheringNodeDataHandler = null,
            string indexDirectoryPath = null) where T : class, IExamineDocument;
    }
}
