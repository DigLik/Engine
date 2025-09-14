using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Engine.Generator;

[Generator]
public class QueryGenerator : IIncrementalGenerator
{
    private const int MaxComponents = 8;
    private const int MaxTotalComponents = 8;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            var forEachDelegates = new StringBuilder();
            for (int i = 1; i <= MaxTotalComponents; i++)
            {
                string delegateParams = GenerateRefDelegateArgs(i, "c");
                string whereClauses = GenerateWhereClauses(i);
                forEachDelegates.AppendLine($"public delegate void ForEachAction<{GenerateGenericParameters(i)}>({delegateParams}) {whereClauses};");
                forEachDelegates.AppendLine($"public delegate void ForEachWithEntityAction<{GenerateGenericParameters(i)}>(Entity entity, {delegateParams}) {whereClauses};");
            }

            AddSourceFile(ctx, "ForEachDelegates.g.cs", "Engine.ECS", null, forEachDelegates.ToString(), "Engine.ECS");
        });

        var maxComponentsProvider = context.CompilationProvider.Select((_, _) => MaxComponents);
        context.RegisterSourceOutput(maxComponentsProvider, GenerateAndAddSources);
    }

    private void GenerateAndAddSources(SourceProductionContext context, int maxComponents)
    {
        var systemBaseMethods = new StringBuilder();
        var fluentBuilderMethods = new StringBuilder();
        var fluentBuilderGenericStructs = new StringBuilder();
        var queryBuilderMethods = new StringBuilder();
        var worldApiMethods = new StringBuilder();
        var worldImplMethods = new StringBuilder();
        var iteratorClasses = new StringBuilder();


        systemBaseMethods.AppendLine("    protected FluentQueryBuilder Query() => new(World, _queryCache);");
        for (int i = 1; i <= maxComponents; i++)
        {
            var generics = GenerateGenericParameters(i);
            var types = GenerateTypeParameters(i);
            var where = GenerateWhereClauses(i);
            systemBaseMethods.AppendLine($"    protected FluentQueryBuilder<{generics}> Query<{generics}>() {where} => new FluentQueryBuilder(World, _queryCache).With<{generics}>();");
        }

        GenerateFluentBuilders(fluentBuilderMethods, fluentBuilderGenericStructs, maxComponents);

        for (int i = 1; i <= MaxTotalComponents; i++)
        {
            string genericParams = GenerateGenericParameters(i);
            string whereClauses = GenerateWhereClauses(i);
            string requiredIds = GenerateTypeIdsArray(i, "_registry.");
            string columnIndices = GenerateColumnIndices(i);
            string columnVars = GenerateColumnVariables(i);
            string refArgs = GenerateRefDelegateArgs(i, "c");
            string callParams = string.Join(", ", Enumerable.Range(1, i).Select(j => $"ref c{j}.Ref(row)"));
            string callParamsWithEntity = $"chunk.Entities[row], {callParams}";

            queryBuilderMethods.AppendLine($"    public Query Build<{genericParams}>() {whereClauses} => BuildInternal(new int[] {{ {requiredIds} }});");
            worldApiMethods.AppendLine($"    QueryIterator<{genericParams}> Iterate<{genericParams}>(Query query) {whereClauses};");
            worldImplMethods.AppendLine($"    public QueryIterator<{genericParams}> Iterate<{genericParams}>(Query query) {whereClauses} => new(query);");

            iteratorClasses.Append($$"""
public readonly ref struct QueryIterator<{{genericParams}}> {{whereClauses}}
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<{{genericParams}}> action)
    {
        foreach (var match in _query.GetMatches())
        {
{{columnIndices}}
            var chunks = match.Archetype.Chunks;
            for (int i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
{{columnVars}}
                for (int row = 0; row < chunk.Count; row++)
                    action({{callParams}});
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<{{genericParams}}> action)
    {
        foreach (var match in _query.GetMatches())
        {
{{columnIndices}}
            var chunks = match.Archetype.Chunks;
            for (int i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
{{columnVars}}
                for (int row = 0; row < chunk.Count; row++)
                    action({{callParamsWithEntity}});
            }
        }
    }
}
""");
        }

        AddSourceFile(context, "SystemBase.Queries.g.cs", "Engine.ECS", "public abstract partial class SystemBase", systemBaseMethods.ToString(), "System.Collections.Generic");
        AddSourceFile(context, "FluentQueryBuilder.g.cs", "Engine.ECS", null, fluentBuilderMethods.ToString(), "System.Collections.Generic", "Engine.ECS.Archetypes");
        AddSourceFile(context, "FluentQueryBuilder.Generic.g.cs", "Engine.ECS", null, fluentBuilderGenericStructs.ToString(), "System", "System.Collections.Generic", "System.Linq", "Engine.ECS.Archetypes");
        AddSourceFile(context, "QueryBuilder.g.cs", "Engine.ECS.Archetypes", "public partial class QueryBuilder", queryBuilderMethods.ToString());
        AddSourceFile(context, "IWorldApi.Iterators.g.cs", "Engine.ECS", "public partial interface IWorldApi", worldApiMethods.ToString(), "Engine.ECS.Archetypes");
        AddSourceFile(context, "ArchetypeWorld.Iterators.g.cs", "Engine.ECS.Archetypes", "public sealed partial class ArchetypeWorld", worldImplMethods.ToString());
        AddSourceFile(context, "QueryIterators.g.cs", "Engine.ECS", null, iteratorClasses.ToString(), "Engine.ECS.Archetypes");
    }

    private void GenerateFluentBuilders(StringBuilder fluentBuilderMethods, StringBuilder fluentBuilderGenericStructs, int maxArity)
    {
        var nonGenericBuilder = new StringBuilder();
        for (int i = 1; i <= maxArity; i++)
        {
            var T = GenerateGenericParameters(i);
            var W = GenerateWhereClauses(i);
            var typeIds = GenerateTypeIdsArray(i, "World.");

            nonGenericBuilder.AppendLine($"    public FluentQueryBuilder<{T}> With<{T}>() {W} {{");
            nonGenericBuilder.AppendLine($"        var newWith = WithIds is null ? new List<int>() : new List<int>(WithIds);");
            nonGenericBuilder.AppendLine($"        newWith.AddRange(new[] {{ {typeIds} }});");
            nonGenericBuilder.AppendLine($"        return new FluentQueryBuilder<{T}>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));");
            nonGenericBuilder.AppendLine($"    }}");

            nonGenericBuilder.AppendLine($"    public FluentQueryBuilder Without<{T}>() {W} {{");
            nonGenericBuilder.AppendLine($"        var newWithout = WithoutIds is null ? new List<int>() : new List<int>(WithoutIds);");
            nonGenericBuilder.AppendLine($"        newWithout.AddRange(new[] {{ {typeIds} }});");
            nonGenericBuilder.AppendLine($"        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);");
            nonGenericBuilder.AppendLine($"    }}");
        }
        fluentBuilderMethods.AppendLine("public readonly partial struct FluentQueryBuilder {");
        fluentBuilderMethods.Append(nonGenericBuilder);
        fluentBuilderMethods.AppendLine("}");

        for (int i = 1; i <= MaxTotalComponents; i++)
        {
            var TCurrent = GenerateGenericParameters(i);
            var WCurrent = GenerateWhereClauses(i);

            fluentBuilderGenericStructs.AppendLine($"public readonly partial struct FluentQueryBuilder<{TCurrent}> {WCurrent}");
            fluentBuilderGenericStructs.AppendLine("{");

            fluentBuilderGenericStructs.AppendLine("    private readonly FluentQueryBuilder _builder;");
            fluentBuilderGenericStructs.AppendLine("    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }");
            fluentBuilderGenericStructs.AppendLine();


            fluentBuilderGenericStructs.AppendLine(GenerateForEachMethod(i, TCurrent, false));
            fluentBuilderGenericStructs.AppendLine(GenerateForEachMethod(i, TCurrent, true));

            for (int j = 1; i + j <= MaxTotalComponents; j++)
            {
                var TNext = GenerateGenericParameters(j, "U");
                var WNext = GenerateWhereClauses(j, "U", "unmanaged");
                var typeIdsNext = GenerateTypeIdsArray(j, "_builder.World.", "U");
                var TCombined = $"{TCurrent}, {TNext}";

                fluentBuilderGenericStructs.AppendLine($"    public FluentQueryBuilder<{TCombined}> With<{TNext}>() {WNext} {{");
                fluentBuilderGenericStructs.AppendLine($"        var newWith = _builder.WithIds is null ? new List<int>() : new List<int>(_builder.WithIds);");
                fluentBuilderGenericStructs.AppendLine($"        newWith.AddRange(new[] {{ {typeIdsNext} }});");
                fluentBuilderGenericStructs.AppendLine($"        return new FluentQueryBuilder<{TCombined}>(new FluentQueryBuilder(_builder.World, _builder.QueryCache, newWith, _builder.WithoutIds));");
                fluentBuilderGenericStructs.AppendLine($"    }}");
            }

            for (int j = 1; j <= maxArity; j++)
            {
                var TNext = GenerateGenericParameters(j, "U");
                var WNext = GenerateWhereClauses(j, "U", "unmanaged");
                var typeIdsNext = GenerateTypeIdsArray(j, "_builder.World.", "U");

                fluentBuilderGenericStructs.AppendLine($"    public FluentQueryBuilder<{TCurrent}> Without<{TNext}>() {WNext} {{");
                fluentBuilderGenericStructs.AppendLine($"        var newWithout = _builder.WithoutIds is null ? new List<int>() : new List<int>(_builder.WithoutIds);");
                fluentBuilderGenericStructs.AppendLine($"        newWithout.AddRange(new[] {{ {typeIdsNext} }});");
                fluentBuilderGenericStructs.AppendLine($"        return new FluentQueryBuilder<{TCurrent}>(new FluentQueryBuilder(_builder.World, _builder.QueryCache, _builder.WithIds, newWithout));");
                fluentBuilderGenericStructs.AppendLine($"    }}");
            }

            fluentBuilderGenericStructs.AppendLine("}");
        }
    }

    private string GenerateForEachMethod(int arity, string generics, bool withEntity)
    {
        var delegateType = withEntity ? $"ForEachWithEntityAction<{generics}>" : $"ForEachAction<{generics}>";
        var types = GenerateTypeParameters(arity);

        return $$"""
    public void ForEach({{delegateType}} action)
    {
        var requiredTypes = new[] { {{types}} };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null)
        {
            withList.AddRange(_builder.WithIds);
        }
        foreach(var type in requiredTypes)
        {
            withList.Add(_builder.World.GetTypeId(type));
        }
        withList.Sort();

        _builder.WithoutIds?.Sort();

        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList)
                hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null)
                foreach (var id in _builder.WithoutIds)
                    hashCode = hashCode * 31 + id;
        }

        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();

            foreach(var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null)
                foreach(var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            
            query = qb.Build<{{generics}}>();
            _builder.QueryCache[hashCode] = query;
        }

        _builder.World.Iterate<{{generics}}>(query).ForEach(action);
    }
""";
    }

    private static void AddSourceFile(SourceProductionContext ctx, string fileName, string ns, string? partialClassHeader, string body, params string[] usings)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();

        sb.AppendLine($"namespace {ns};");
        sb.AppendLine();

        foreach (var u in usings) sb.AppendLine($"using {u};");
        if (usings.Length > 0) sb.AppendLine();

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

    private static void AddSourceFile(IncrementalGeneratorPostInitializationContext ctx, string fileName, string ns, string? partialClassHeader, string body, params string[] usings)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();

        sb.AppendLine($"namespace {ns};");
        sb.AppendLine();

        foreach (var u in usings) sb.AppendLine($"using {u};");
        if (usings.Length > 0) sb.AppendLine();

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
    private static string GenerateGenericParameters(int count, string prefix = "T") => string.Join(", ", Enumerable.Range(1, count).Select(i => $"{prefix}{i}"));
    private static string GenerateTypeParameters(int count, string prefix = "T") => string.Join(", ", Enumerable.Range(1, count).Select(i => $"typeof({prefix}{i})"));
    private static string GenerateWhereClauses(int count, string prefix = "T", string constraint = "unmanaged") => string.Join(" ", Enumerable.Range(1, count).Select(i => $"where {prefix}{i} : {constraint}"));
    private static string GenerateTypeIdsArray(int count, string registryPrefix, string typePrefix = "T") => string.Join(", ", Enumerable.Range(1, count).Select(i => $"{registryPrefix}GetTypeId<{typePrefix}{i}>()"));
    private static string GenerateColumnIndices(int count) => string.Join("\n", Enumerable.Range(1, count).Select(i => $"            int colIdx{i} = match.ColumnIndices[{i - 1}];"));
    private static string GenerateColumnVariables(int count) => string.Join("\n", Enumerable.Range(1, count).Select(i => $"                var c{i} = (Column<T{i}>)chunk.Columns[colIdx{i}];"));
    private static string GenerateRefDelegateArgs(int count, string namePrefix) => string.Join(", ", Enumerable.Range(1, count).Select(i => $"ref T{i} {namePrefix}{i}"));
    #endregion
}