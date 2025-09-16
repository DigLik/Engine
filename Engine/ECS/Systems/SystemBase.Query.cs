namespace Engine.ECS;

public abstract partial class SystemBase
{
    protected FluentQueryBuilder Query() => QueryRegistry.Query();
    protected FluentQueryBuilder<T1> Query<T1>() where T1 : unmanaged => QueryRegistry.Query<T1>();
    protected FluentQueryBuilder<T1, T2> Query<T1, T2>() where T1 : unmanaged where T2 : unmanaged => QueryRegistry.Query<T1, T2>();
    protected FluentQueryBuilder<T1, T2, T3> Query<T1, T2, T3>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged => QueryRegistry.Query<T1, T2, T3>();
    protected FluentQueryBuilder<T1, T2, T3, T4> Query<T1, T2, T3, T4>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged => QueryRegistry.Query<T1, T2, T3, T4>();
    protected FluentQueryBuilder<T1, T2, T3, T4, T5> Query<T1, T2, T3, T4, T5>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged => QueryRegistry.Query<T1, T2, T3, T4, T5>();
    protected FluentQueryBuilder<T1, T2, T3, T4, T5, T6> Query<T1, T2, T3, T4, T5, T6>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged => QueryRegistry.Query<T1, T2, T3, T4, T5, T6>();
    protected FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> Query<T1, T2, T3, T4, T5, T6, T7>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged => QueryRegistry.Query<T1, T2, T3, T4, T5, T6, T7>();
    protected FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> Query<T1, T2, T3, T4, T5, T6, T7, T8>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged => QueryRegistry.Query<T1, T2, T3, T4, T5, T6, T7, T8>();
}