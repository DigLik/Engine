using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Engine.Generator;

[Generator]
public class QueryGenerator : IIncrementalGenerator
{
    private const int MaxComponents = 8;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var maxComponentsProvider = context.CompilationProvider.Select((_, _) => MaxComponents);
        context.RegisterSourceOutput(maxComponentsProvider, GenerateAndAddSources);
    }

    private void GenerateAndAddSources(SourceProductionContext context, int maxComponents)
    {
        var forEachDelegates = new StringBuilder();
        var builderMethods = new StringBuilder();
        var iteratorClasses = new StringBuilder();
        var iteratorApiMethods = new StringBuilder();
        var iteratorWorldMethods = new StringBuilder();
        var systemBaseMethods = new StringBuilder();

        for (int i = 1; i <= maxComponents; i++)
        {
            string genericParams = GenerateGenericParameters(i);
            string whereClauses = GenerateWhereClauses(i);
            string requiredIds = GenerateTypeIdsArray(i, "_registry.");
            string columnIndices = GenerateColumnIndices(i);
            string delegateParams = GenerateDelegateParameters(i);
            string columnVars = GenerateColumnVariables(i);

            string forEachActionDelegateName = $"ForEachAction<{delegateParams}>";
            string forEachActionDelegateParams = GenerateRefDelegateArgs(i, "c");
            string forEachActionCallParams = string.Join(", ", Enumerable.Range(1, i).Select(j => $"ref c{j}.Ref(row)"));

            string forEachWithEntityActionDelegateName = $"ForEachWithEntityAction<{delegateParams}>";
            string forEachWithEntityActionDelegateParams = $"Entity entity, {forEachActionDelegateParams}";
            string forEachWithEntityActionCallParams = $"chunk.Entities[row], {forEachActionCallParams}";

            string typeParamsString = string.Join(", ", Enumerable.Range(1, i).Select(j => $"typeof(T{j})"));

            forEachDelegates.AppendLine($"public delegate void {forEachActionDelegateName}({forEachActionDelegateParams}) {whereClauses};");
            forEachDelegates.AppendLine($"public delegate void {forEachWithEntityActionDelegateName}({forEachWithEntityActionDelegateParams}) {whereClauses};");

            builderMethods.AppendLine($"    public Query Build<{genericParams}>() {whereClauses}");
            builderMethods.AppendLine("    {");
            builderMethods.AppendLine($"        return BuildInternal(new int[] {{ {requiredIds} }});");
            builderMethods.AppendLine("    }");
            builderMethods.AppendLine();

            iteratorApiMethods.AppendLine($"    QueryIterator<{genericParams}> Iterate<{genericParams}>(Query query) {whereClauses};");
            iteratorWorldMethods.AppendLine($"    public QueryIterator<{genericParams}> Iterate<{genericParams}>(Query query) {whereClauses}");
            iteratorWorldMethods.AppendLine("    {");
            iteratorWorldMethods.AppendLine("        return new(query);");
            iteratorWorldMethods.AppendLine("    }");

            iteratorClasses.Append($$"""
public readonly ref struct QueryIterator<{{genericParams}}> {{whereClauses}}
{
    private readonly Query _query;

    internal QueryIterator(Query query)
    {
        _query = query;
    }

    public void ForEach({{forEachActionDelegateName}} action)
    {
        var matches = _query.GetMatches();
        foreach (var match in matches)
        {
{{columnIndices}}
            foreach (var chunk in match.Archetype.Chunks)
            {
{{columnVars}}
                for (int row = 0; row < chunk.Count; row++)
                {
                    action({{forEachActionCallParams}});
                }
            }
        }
    }

    public void ForEach({{forEachWithEntityActionDelegateName}} action)
    {
        var matches = _query.GetMatches();
        foreach (var match in matches)
        {
{{columnIndices}}
            foreach (var chunk in match.Archetype.Chunks)
            {
{{columnVars}}
                for (int row = 0; row < chunk.Count; row++)
                {
                    action({{forEachWithEntityActionCallParams}});
                }
            }
        }
    }
}

""");
            systemBaseMethods.AppendLine($"    protected QueryIterator<{genericParams}> Query<{genericParams}>() {whereClauses}");
            systemBaseMethods.AppendLine("    {");
            systemBaseMethods.AppendLine($"        int hashCode = HashCode.Combine({typeParamsString});");
            systemBaseMethods.AppendLine("        if (!_queryCache.TryGetValue(hashCode, out var query))");
            systemBaseMethods.AppendLine("        {");
            systemBaseMethods.AppendLine($"            query = World.Builder().Build<{genericParams}>();");
            systemBaseMethods.AppendLine("            _queryCache[hashCode] = query;");
            systemBaseMethods.AppendLine("        }");
            systemBaseMethods.AppendLine($"        return World.Iterate<{genericParams}>(query);");
            systemBaseMethods.AppendLine("    }");
            systemBaseMethods.AppendLine();
        }

        AddSourceFile(context, "ForEachDelegates.g.cs", "Engine.ECS.Delegates", null, forEachDelegates.ToString(), "using Engine.ECS;");
        AddSourceFile(context, "QueryBuilder.g.cs", "Engine.ECS.Archetypes", "public partial class QueryBuilder", builderMethods.ToString(), "using Engine.ECS.Archetypes;");
        AddSourceFile(context, "IWorldApi.Iterators.g.cs", "Engine.ECS", "public partial interface IWorldApi", iteratorApiMethods.ToString(), "using Engine.ECS.Iterators;", "using Engine.ECS.Archetypes;");
        AddSourceFile(context, "ArchetypeWorld.Iterators.g.cs", "Engine.ECS.Archetypes", "public sealed partial class ArchetypeWorld", iteratorWorldMethods.ToString(), "using Engine.ECS.Iterators;");
        AddSourceFile(context, "QueryIterators.g.cs", "Engine.ECS.Iterators", null, iteratorClasses.ToString(), "using Engine.ECS;", "using Engine.ECS.Delegates;", "using Engine.ECS.Archetypes;");
        AddSourceFile(context, "SystemBase.Queries.g.cs", "Engine.ECS", "public abstract partial class SystemBase", systemBaseMethods.ToString(), "using Engine.ECS.Iterators;");
    }

    private static void AddSourceFile(SourceProductionContext ctx, string fileName, string ns, string? partialClassHeader, string body, params string[] usings)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        foreach (var u in usings)
        {
            sb.AppendLine(u);
        }
        if (usings.Length > 0)
        {
            sb.AppendLine();
        }

        sb.AppendLine($"namespace {ns};");
        sb.AppendLine();
        if (partialClassHeader != null)
        {
            sb.AppendLine(partialClassHeader);
            sb.AppendLine("{");
            var indentedBody = "    " + body.Replace("\n", "\n    ").TrimEnd();
            sb.Append(indentedBody);
            sb.AppendLine();
            sb.AppendLine("}");
        }
        else
        {
            sb.Append(body);
        }

        ctx.AddSource(fileName, SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    #region Helper Methods
    private static string GenerateGenericParameters(int count) => string.Join(", ", Enumerable.Range(1, count).Select(i => $"T{i}"));
    private static string GenerateDelegateParameters(int count) => string.Join(", ", Enumerable.Range(1, count).Select(i => $"T{i}"));
    private static string GenerateWhereClauses(int count) => string.Join(" ", Enumerable.Range(1, count).Select(i => $"where T{i} : unmanaged"));
    private static string GenerateTypeIdsArray(int count, string registryPrefix) => string.Join(", ", Enumerable.Range(1, count).Select(i => $"{registryPrefix}GetTypeId<T{i}>()"));
    private static string GenerateColumnIndices(int count) => string.Join("\n", Enumerable.Range(1, count).Select(i => $"            int colIdx{i} = match.ColumnIndices[{i - 1}];"));
    private static string GenerateColumnVariables(int count) => string.Join("\n", Enumerable.Range(1, count).Select(i => $"                var c{i} = (Column<T{i}>)chunk.Columns[colIdx{i}];"));
    private static string GenerateRefDelegateArgs(int count, string prefix) => string.Join(", ", Enumerable.Range(1, count).Select(i => $"ref T{i} {prefix}{i}"));
    #endregion
}