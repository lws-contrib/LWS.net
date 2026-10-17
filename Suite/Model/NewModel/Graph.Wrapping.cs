namespace Model.NewModel;

public class Graph(IGraph original) : WrapperGraph(original)
{
    public Manifest Manifest => this.SubjectsOf(Vocabulary.Test, Manifest.Wrap).Union(this.SubjectsOf(Vocabulary.Include, Manifest.Wrap)).Single();
}
