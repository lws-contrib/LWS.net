namespace Model;

public static class TemplateExpander
{
    private const string ParamNamespace = "urn:lws:param:";

    public static void Expand(IGraph graph)
    {
        var stepTemplates = new NewModel.Graph(graph).StepTemplates.ToArray();
        var templates = stepTemplates.ToDictionary(t => (INode)t, ReadTemplate);

        var instances = stepTemplates
            .SelectMany(template => template.Instances, (template, instance) => (Instance: (INode)instance, Template: (INode)template))
            .Where(pair => !templates.ContainsKey(pair.Instance))
            .GroupBy(pair => pair.Instance)
            .ToDictionary(g => g.Key, g => g.First().Template);

        var arguments = instances.Keys.ToDictionary(instance => instance, instance => NewModel.TemplateInstance.Wrap(instance, graph).Arguments.ToArray());

        foreach (var (instance, templateNode) in instances)
        {
            new Instantiation(graph, templateNode, templates[templateNode], instance, arguments[instance]).Run();
        }

        foreach (var node in templates.Keys)
        {
            var stepTemplate = NewModel.StepTemplate.Wrap(node, graph);
            stepTemplate.Expansion = null;
            stepTemplate.Parameters.Clear();
            stepTemplate.Type.Remove(((IUriNode)NewModel.Vocabulary.StepTemplate).Uri);
        }

        foreach (var (instance, templateNode) in instances)
        {
            var templateInstance = NewModel.TemplateInstance.Wrap(instance, graph);
            templateInstance.Arguments.Clear();
            templateInstance.Type.Remove(((IUriNode)templateNode).Uri);
        }
    }

    private static Template ReadTemplate(NewModel.StepTemplate template)
    {
        var expansion = template.Expansion
            ?? throw new TemplateExpansionException($"Template {template} has no expansion.");

        return new Template([.. template.Parameters], expansion);
    }

    private sealed record Template(NewModel.Parameter[] Parameters, INode Expansion);

    private sealed class Instantiation(IGraph graph, INode templateNode, Template template, INode instance, NewModel.Argument[] arguments)
    {
        private readonly Dictionary<INode, INode> blanks = new() { [template.Expansion] = instance };

        private readonly HashSet<INode> visited = [];

        public void Run()
        {
            foreach (var argument in arguments.Where(a => template.Parameters.All(p => p.ParameterName != a.ParameterName)))
            {
                throw Error("Argument uses undeclared parameter", argument.ParameterName!);
            }

            foreach (var parameter in template.Parameters.Where(p => p.Required == true && p.Default is null))
            {
                Resolve(parameter.ParameterName!);
            }

            Copy(template.Expansion);
        }

        private void Copy(INode node)
        {
            if (!visited.Add(node))
            {
                return;
            }

            foreach (var triple in graph.GetTriplesWithSubject(node).ToArray())
            {
                graph.Assert(new Triple(Map(triple.Subject), Map(triple.Predicate), Map(triple.Object)));

                if (triple.Object.NodeType == NodeType.Blank && Placeholder(triple.Object) is null)
                {
                    Copy(triple.Object);
                }
            }
        }

        private INode Map(INode node)
        {
            switch (node.NodeType)
            {
                case NodeType.Uri when ((IUriNode)node).Uri.AbsoluteUri is var uri && uri.StartsWith(ParamNamespace, StringComparison.Ordinal):
                    return Resolve(uri[ParamNamespace.Length..]);
                case NodeType.Blank:
                    if (Placeholder(node) is { } name)
                    {
                        return Resolve(name);
                    }

                    if (!blanks.TryGetValue(node, out var mapped))
                    {
                        blanks[node] = mapped = graph.CreateBlankNode();
                    }

                    return mapped;
                default:
                    return node;
            }
        }

        private string? Placeholder(INode node) =>
            graph.GetTriplesWithSubject(node).ToArray() is [{ Object.NodeType: NodeType.Literal }] ? NewModel.Placeholder.Wrap(node, graph).ParameterName : null;

        private INode Resolve(string name)
        {
            var parameter = template.Parameters.FirstOrDefault(p => p.ParameterName == name)
                ?? throw Error("Placeholder uses undeclared parameter", name);

            var values = arguments.Where(a => a.ParameterName == name).Select(a => a.Value!).ToArray();

            return values switch
            {
                [var value] => value,
                [] => parameter.Default ?? throw Error("Parameter has no value and no default", name),
                _ => throw Error("Parameter has more than one value", name),
            };
        }

        private TemplateExpansionException Error(string message, string name) =>
            new($"{message} (template {templateNode}, instance {instance}, parameter '{name}').");
    }
}
