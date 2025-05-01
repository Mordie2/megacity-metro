using Unity.Entities;
using Unity.Mathematics;

public struct LaserHitSoundRequest : IComponentData
{
    public Entity Entity;
    public float3 Position;
    public bool isGettingHit;
}
