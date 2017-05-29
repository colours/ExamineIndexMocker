namespace ExamineIndex.Mocker.Exposed.Implementation
{
    using Interfaces;
    using Internal.Factories;

    /// <summary>
    /// Defines an entry point to the examine index mocker functionality.
    /// </summary>
    public static class Context
    {
        /// <summary>
        /// Gets the examine index factory.
        /// </summary>
        /// <returns></returns>
        public static IExamineIndexFactory GetExamineIndexFactory()
        {
            return new ExamineIndexFactory();
        }
    }
}
