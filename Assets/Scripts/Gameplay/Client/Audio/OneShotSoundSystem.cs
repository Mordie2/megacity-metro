using Unity.Entities;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Unity.Collections;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.LocalSimulation)]
public partial class OneShotSoundSystem : SystemBase
{
    private EntityQuery _soundQuery;

    protected override void OnCreate()
    {
        _soundQuery = GetEntityQuery(ComponentType.ReadOnly<SoundOneShotRequest>());
    }

    protected override void OnUpdate()
    {
        var requests = _soundQuery.ToComponentDataArray<SoundOneShotRequest>(Allocator.Temp);

        foreach (var request in requests)
        {
            var instance = RuntimeManager.CreateInstance(request.EventPath.ToString());
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(request.Position));
            instance.start();
            instance.release();

        }

        if (!_soundQuery.IsEmptyIgnoreFilter)
        {
            EntityManager.RemoveComponent<SoundOneShotRequest>(_soundQuery);
        }
    }
}
