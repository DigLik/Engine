namespace Engine.ECS;

public delegate void ForEachAction<T1>(ref T1 c1) where T1 : unmanaged;
public delegate void ForEachWithEntityAction<T1>(Entity entity, ref T1 c1) where T1 : unmanaged;
public delegate void ForEachAction<T1, T2>(ref T1 c1, ref T2 c2) where T1 : unmanaged where T2 : unmanaged;
public delegate void ForEachWithEntityAction<T1, T2>(Entity entity, ref T1 c1, ref T2 c2) where T1 : unmanaged where T2 : unmanaged;
public delegate void ForEachAction<T1, T2, T3>(ref T1 c1, ref T2 c2, ref T3 c3) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged;
public delegate void ForEachWithEntityAction<T1, T2, T3>(Entity entity, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged;
public delegate void ForEachAction<T1, T2, T3, T4>(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged;
public delegate void ForEachWithEntityAction<T1, T2, T3, T4>(Entity entity, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged;
public delegate void ForEachAction<T1, T2, T3, T4, T5>(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4, ref T5 c5) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged;
public delegate void ForEachWithEntityAction<T1, T2, T3, T4, T5>(Entity entity, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4, ref T5 c5) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged;
public delegate void ForEachAction<T1, T2, T3, T4, T5, T6>(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4, ref T5 c5, ref T6 c6) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged;
public delegate void ForEachWithEntityAction<T1, T2, T3, T4, T5, T6>(Entity entity, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4, ref T5 c5, ref T6 c6) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged;
public delegate void ForEachAction<T1, T2, T3, T4, T5, T6, T7>(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4, ref T5 c5, ref T6 c6, ref T7 c7) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged;
public delegate void ForEachWithEntityAction<T1, T2, T3, T4, T5, T6, T7>(Entity entity, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4, ref T5 c5, ref T6 c6, ref T7 c7) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged;
public delegate void ForEachAction<T1, T2, T3, T4, T5, T6, T7, T8>(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4, ref T5 c5, ref T6 c6, ref T7 c7, ref T8 c8) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged;
public delegate void ForEachWithEntityAction<T1, T2, T3, T4, T5, T6, T7, T8>(Entity entity, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4, ref T5 c5, ref T6 c6, ref T7 c7, ref T8 c8) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged;