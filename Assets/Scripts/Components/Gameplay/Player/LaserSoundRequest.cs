using Unity.Entities;
using Unity.Mathematics;

public struct LaserSoundRequest : IComponentData
{
    public Entity Entity;
    public float3 Position;
    public bool IsFiring;
}
