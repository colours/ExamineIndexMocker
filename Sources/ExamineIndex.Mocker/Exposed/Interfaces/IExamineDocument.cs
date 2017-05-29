namespace ExamineIndex.Mocker.Exposed.Interfaces
{
    /// <summary>
    /// Defines an important examine field that should be present in each document during indexing.
    /// </summary>
    public interface IExamineDocument
    {
        /// <summary>
        /// The examine document Id. This property appears in the index as a "__NodeId" field of the string type.
        /// You can also have an extra field for this property if you use ExamineFieldAttribute with a different field name.
        /// </summary>
        int Id { get; }
    }
}
