namespace ExamineIndex.Mocker.Exposed.Interfaces
{
    using Examine.LuceneEngine.Providers;

    /// <summary>
    /// Incapsulates the examine index objects.
    /// </summary>
    public interface IExamineIndex
    {
        /// <summary>
        /// Gets the lucene searcher provider.
        /// </summary>
        LuceneSearcher LuceneSearcherProvider { get; }
    }
}
