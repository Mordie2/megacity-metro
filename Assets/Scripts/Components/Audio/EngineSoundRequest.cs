using Unity.Entities;
using Unity.Mathematics;

public struct EngineSoundRequest : IComponentData
{
    public Entity Entity;
    public float3 Position;
    public float IdleFactor;
    public float DamageFactor;
    public bool IsAlive;
}
