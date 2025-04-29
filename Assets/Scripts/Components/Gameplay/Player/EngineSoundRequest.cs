using Unity.Entities;
using Unity.Mathematics;

public struct EngineSoundRequest : IComponentData
{
    public Entity Entity;
    public float3 Position;
    public float IdleFactor; // 0 = idle, 1 = full speed
    public bool IsAlive;
}
