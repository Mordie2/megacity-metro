using Unity.Entities;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Unity.Collections;
using System.Collections.Generic;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.LocalSimulation)]
public partial class LaserHitSoundSystem : SystemBase
{
    private EntityQuery _hitQuery;
    private Dictionary<Entity, EventInstance> _activeHits;

    protected override void OnCreate()
    {
        _hitQuery = GetEntityQuery(ComponentType.ReadOnly<LaserHitSoundRequest>());
        _activeHits = new Dictionary<Entity, EventInstance>();
    }

    protected override void OnUpdate()
    {
        var requests = _hitQuery.ToComponentDataArray<LaserHitSoundRequest>(Allocator.Temp);

        foreach (var request in requests)
        {
            var entity = request.Entity;

            if (request.isGettingHit)
            {
                if (!_activeHits.TryGetValue(entity, out var instance))
                {
                    instance = RuntimeManager.CreateInstance(FMODEvents.instance.LaserHit);
                    instance.set3DAttributes(RuntimeUtils.To3DAttributes(request.Position));
                    instance.start();
                    instance.release();
                    _activeHits[entity] = instance;

                    // Debug.Log($"[LaserHitSoundSystem] Started hit sound for Entity {entity.Index}");
                }
                else
                {
                    instance.set3DAttributes(RuntimeUtils.To3DAttributes(request.Position));
                }
            }
            else
            {
                if (_activeHits.TryGetValue(entity, out var instance))
                {
                    instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    _activeHits.Remove(entity);

                    // Debug.Log($"[LaserHitSoundSystem] Stopped hit sound for Entity {entity.Index}");
                }
            }
        }

        if (!_hitQuery.IsEmptyIgnoreFilter)
        {
            EntityManager.RemoveComponent<LaserHitSoundRequest>(_hitQuery);
        }
    }

    protected override void OnDestroy()
    {
        foreach (var kvp in _activeHits)
        {
            kvp.Value.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            kvp.Value.release();
        }
        _activeHits.Clear();
    }
}
