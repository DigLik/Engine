namespace Engine.ECS.Archetypes;

public partial class QueryBuilder
{
    public Query Build<T1>() where T1 : unmanaged => BuildInternal([_registry.GetTypeId<T1>()]);
    public Query Build<T1, T2>() where T1 : unmanaged where T2 : unmanaged => BuildInternal([_registry.GetTypeId<T1>(), _registry.GetTypeId<T2>()]);
    public Query Build<T1, T2, T3>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged => BuildInternal([_registry.GetTypeId<T1>(), _registry.GetTypeId<T2>(), _registry.GetTypeId<T3>()]);
    public Query Build<T1, T2, T3, T4>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged => BuildInternal([_registry.GetTypeId<T1>(), _registry.GetTypeId<T2>(), _registry.GetTypeId<T3>(), _registry.GetTypeId<T4>()]);
    public Query Build<T1, T2, T3, T4, T5>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged => BuildInternal([_registry.GetTypeId<T1>(), _registry.GetTypeId<T2>(), _registry.GetTypeId<T3>(), _registry.GetTypeId<T4>(), _registry.GetTypeId<T5>()]);
    public Query Build<T1, T2, T3, T4, T5, T6>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged => BuildInternal([_registry.GetTypeId<T1>(), _registry.GetTypeId<T2>(), _registry.GetTypeId<T3>(), _registry.GetTypeId<T4>(), _registry.GetTypeId<T5>(), _registry.GetTypeId<T6>()]);
    public Query Build<T1, T2, T3, T4, T5, T6, T7>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged => BuildInternal([_registry.GetTypeId<T1>(), _registry.GetTypeId<T2>(), _registry.GetTypeId<T3>(), _registry.GetTypeId<T4>(), _registry.GetTypeId<T5>(), _registry.GetTypeId<T6>(), _registry.GetTypeId<T7>()]);
    public Query Build<T1, T2, T3, T4, T5, T6, T7, T8>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged => BuildInternal([_registry.GetTypeId<T1>(), _registry.GetTypeId<T2>(), _registry.GetTypeId<T3>(), _registry.GetTypeId<T4>(), _registry.GetTypeId<T5>(), _registry.GetTypeId<T6>(), _registry.GetTypeId<T7>(), _registry.GetTypeId<T8>()]);
}