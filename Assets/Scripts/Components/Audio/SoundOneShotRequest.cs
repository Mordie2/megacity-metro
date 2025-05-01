using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections; // Needed for FixedString64Bytes

public struct SoundOneShotRequest : IComponentData
{
    public FixedString64Bytes EventPath; // use FMOD event path as string
    public float3 Position;
}

