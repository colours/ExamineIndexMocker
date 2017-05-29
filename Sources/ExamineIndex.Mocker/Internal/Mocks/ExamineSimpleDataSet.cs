namespace ExamineIndex.Mocker.Internal.Mocks
{
    using System.Collections;
    using System.Collections.Generic;

    using Examine;
    using Examine.LuceneEngine;

    internal class ExamineSimpleDataSet : IEnumerable<SimpleDataSet>
    {
        private List<SimpleDataSet> Source { get; }

        private string Type { get; }

        internal ExamineSimpleDataSet(string type)
        {
            this.Source = new List<SimpleDataSet>();
            this.Type = type;
        }

        internal ExamineSimpleDataSet(string type, IEnumerable<SimpleDataSet> source)
        {
            this.Source = new List<SimpleDataSet>(source);
            this.Type = type;
        }

        internal ExamineSimpleDataSet AddData(int id, string name, string value)
        {
            var nodeDefinition = new IndexedNode { NodeId = id, Type = this.Type };

            var rowData = new Dictionary<string, string>
            {
                {name, value}
            };

            this.Source.Add(
                new SimpleDataSet
                {
                    NodeDefinition = nodeDefinition,
                    RowData = rowData
                });
            return this;
        }

        public IEnumerator<SimpleDataSet> GetEnumerator()
        {
            return this.Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Source.GetEnumerator();
        }
    }
}