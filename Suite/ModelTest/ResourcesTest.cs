using Model;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace ModelTest;

[TestClass]
public class ResourcesTest
{
    private static readonly Uri NewUri = new(Resources.Base, "new.ttl");

    [TestMethod]
    public void ParseLeavesManifestWithoutTemplatesUnchanged()
    {
        var plain = new Graph { BaseUri = NewUri };
        new TurtleParser().Load(plain, Resources.Reader("new.ttl"));

        var actual = Resources.Parse(NewUri);

        Assert.IsTrue(plain.Equals(actual));
    }
}
