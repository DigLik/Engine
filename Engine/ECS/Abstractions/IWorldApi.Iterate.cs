using Engine.ECS.Archetypes.QueryDefinition;
using Engine.ECS.Querying;

namespace Engine.ECS.Abstractions;

public partial interface IWorldApi
{
    QueryIterator<T1> Iterate<T1>(Query query) where T1 : unmanaged;
    QueryIterator<T1, T2> Iterate<T1, T2>(Query query) where T1 : unmanaged where T2 : unmanaged;
    QueryIterator<T1, T2, T3> Iterate<T1, T2, T3>(Query query) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged;
    QueryIterator<T1, T2, T3, T4> Iterate<T1, T2, T3, T4>(Query query) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged;
    QueryIterator<T1, T2, T3, T4, T5> Iterate<T1, T2, T3, T4, T5>(Query query) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged;
    QueryIterator<T1, T2, T3, T4, T5, T6> Iterate<T1, T2, T3, T4, T5, T6>(Query query) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged;
    QueryIterator<T1, T2, T3, T4, T5, T6, T7> Iterate<T1, T2, T3, T4, T5, T6, T7>(Query query) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged;
    QueryIterator<T1, T2, T3, T4, T5, T6, T7, T8> Iterate<T1, T2, T3, T4, T5, T6, T7, T8>(Query query) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged;
}