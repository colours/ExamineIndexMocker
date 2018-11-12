# ExamineIndexMocker

##What is Examine Index Mocker?

This is a .NET framework for MOQing Examine/Lucene index, i.e. it lets create a custom lucene index using a custom model collection. The framework is designed to help make unit tests for Examine/Lucene search implementations thus reducing time for testing and making the code error-tolerant. The framework allows to control which fields of the model should appear in the index, which types they should be, which analyzer to use and makes it possible to handle GatheringNodeData and DocumentWriting events. All that allows to replicate real indexing environment in your tests.

##Features
* The index is created based on the custom model collection. Each property of the model object marked with an ExamineFieldAttribute attribute appears as a field in the index document.
* The attribute allows to control the name of the field, its type and the sorting option.
* The atributes can be placed either on class properties or interface properties. So if a model implements an interface with attributes set on the property declarations - no need to set the same on the class level.
* It is possible to set multiple attributes on a single property - as a result the same numeber of fields will be created in the document (if the names of the attributes are different).
* The empty (null) properties do not get into the index thus making it possible to use one model for multiple tests and create only those fields in the index which are needed per particular test.
* It is possible to set a proper analyzer for each index.
* Also it is possible to provide custom handlers for GatheringNodeData and DocumentWriting events thus controlling the indexing process.
* The index can be created either in memory or on disk (if you further want to examine it further with Luke).
* The framework allows to exclude/include fields.

##Usage

First, a model type should be created. The model should inherit from ExamineIndex.Mocker.Exposed.Interfaces.IExamineDocument interface and has properties marked by ExamineIndex.Mocker.Exposed.Implementation.ExamineFieldAttribute attributes. Example:

```cs
    public class Model : IExamineDocument
    {
        // No need to mark this field with ExamineFieldAttribute - it will appear in the index as Id field automatically under the name of __NodeId.
		// However, you can place the ExamineFieldAttribute on it if a different name (or type) for Id field is needed.
        public int Id { get; set; }

        // The field with the same name as the name of the property will be created in the index. The casing matters.
		[ExamineField]
        public string Text { get; set; }

        // The field with the name as specified by the FieldName property of the attribute will be created in the index with the type specified by ExamineFieldType property.
		[ExamineField(FieldName = "count", ExamineFieldType = ExamineFieldType.Int)]
        public int? Count { get; set; }
    }
```

Second, create a collection of your model objects for specific test. Example:

```cs
    var model = new[]
    {
        new Model {Id = 1, Text = "First"},
        new Model {Id = 2, Text = "Second"},
        new Model {Id = 3, Text = "Third"}
    };
```

Third, create the index based on the model. Example:

```cs
    var index = Context.GetExamineIndexFactory().Get(model, new StandardAnalyzer(Version.LUCENE_29));
```
Forth, search. Example:

```cs
    var index = Context.GetExamineIndexFactory().Get(model, new StandardAnalyzer(Version.LUCENE_29));
```