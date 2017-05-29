namespace ExamineIndex.Mocker.Internal.Mocks
{
    using System;
    using System.Collections.Generic;

    using Examine;

    using Rhino.Mocks;

    internal class ExamineIndexFieldList : IEnumerable<IIndexField>
    {
        private List<IIndexField> IndexFieldList { get; }

        internal ExamineIndexFieldList()
        {
            this.IndexFieldList = new List<IIndexField>();
        }

        internal ExamineIndexFieldList AddIndexField(string name, bool enableSorting)
        {
            return this.AddIndexField(name, string.Empty, enableSorting);
        }

        internal ExamineIndexFieldList AddIndexField(string name)
        {
            return this.AddIndexField(name, string.Empty);
        }

        internal ExamineIndexFieldList AddIndexField(string name, string type, bool enableSorting = false)
        {
            if (type == null)
            {
                type = string.Empty;
            }

            var indexField = MockRepository.GenerateMock<IIndexField>();

            indexField.Stub(p => p.Name).Return(name);
            indexField.Stub(p => p.EnableSorting).Return(enableSorting);
            indexField.Stub(p => p.Type).Return(type);

            this.IndexFieldList.Add(indexField);

            return this;
        }

        public IEnumerator<IIndexField> GetEnumerator()
        {
            return this.IndexFieldList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.IndexFieldList.GetEnumerator();
        }

        public bool ContainsField(string fieldName)
        {
            bool fieldExists =
                this.IndexFieldList.Exists(p => string.Equals(p.Name, fieldName, StringComparison.Ordinal));
            return fieldExists;
        }
    }
}