using Unity.Entities;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Unity.Collections;
using System.Collections.Generic;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.LocalSimulation)]
public partial class LaserSoundSystem : SystemBase
{
    private EntityQuery _laserQuery;
    private Dictionary<Entity, EventInstance> _activeLasers;


    protected override void OnCreate()
    {
        _laserQuery = GetEntityQuery(ComponentType.ReadOnly<LaserSoundRequest>());
        _activeLasers = new Dictionary<Entity, EventInstance>();
    }

    protected override void OnUpdate()
{
    var requests = _laserQuery.ToComponentDataArray<LaserSoundRequest>(Allocator.Temp);

    foreach (var request in requests)
    {
        var entity = request.Entity;

        if (request.IsFiring)
        {
            if (!_activeLasers.TryGetValue(entity, out var instance))
            {
                instance = RuntimeManager.CreateInstance(FMODEvents.instance.ShipLaser);
                instance.set3DAttributes(RuntimeUtils.To3DAttributes(request.Position));
                instance.start();
                instance.release();
                _activeLasers[entity] = instance;

                // Debug.Log($"[LaserSoundSystem] Start laser sound for Entity {entity.Index}");
            }
            else
            {
                instance.set3DAttributes(RuntimeUtils.To3DAttributes(request.Position));
            }
        }
        else
        {
            if (_activeLasers.TryGetValue(entity, out var instance))
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                _activeLasers.Remove(entity);

                //Debug.Log($"[LaserSoundSystem] Stopped laser sound for Entity {entity.Index}");
            }
        }
    }

    if (!_laserQuery.IsEmptyIgnoreFilter)
    {
        EntityManager.RemoveComponent<LaserSoundRequest>(_laserQuery);
    }
}


    protected override void OnDestroy()
    {
        foreach (var kvp in _activeLasers)
        {
            kvp.Value.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            kvp.Value.release();
        }
        _activeLasers.Clear();
    }
}
