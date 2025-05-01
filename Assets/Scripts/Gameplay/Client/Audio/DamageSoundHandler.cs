using Unity.Entities;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Unity.Collections;
using System.Collections.Generic;
using Unity.NetCode;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.LocalSimulation)]
public partial class DamageSoundSystem : SystemBase
{
    private EntityQuery _damageQuery;
    private Dictionary<Entity, EventInstance> _activeDamageInstances;

    protected override void OnCreate()
    {
        _damageQuery = GetEntityQuery(ComponentType.ReadOnly<DamageSoundRequest>());
        _activeDamageInstances = new Dictionary<Entity, EventInstance>();
    }

    protected override void OnUpdate()
    {
        var entities = _damageQuery.ToEntityArray(Allocator.Temp);
        var requests = _damageQuery.ToComponentDataArray<DamageSoundRequest>(Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            var entity = entities[i];
            var request = requests[i];

            if (request.Play)
            {
                if (!_activeDamageInstances.TryGetValue(entity, out var instance))
                {
                    instance = AudioManager.instance.CreateInstance(FMODEvents.instance.Damage);
                    instance.start();
                    _activeDamageInstances[entity] = instance;
                    Debug.Log($"[DamageSoundSystem] Started for {entity.Index}");
                }
            }
            else
            {
                if (_activeDamageInstances.TryGetValue(entity, out var instance))
                {
                    instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    instance.release();
                    _activeDamageInstances.Remove(entity);
                    Debug.Log($"[DamageSoundSystem] Stopped for {entity.Index}");
                }
            }
        }

        entities.Dispose();
        requests.Dispose();
    }

    protected override void OnDestroy()
    {
        foreach (var instance in _activeDamageInstances.Values)
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }
        _activeDamageInstances.Clear();
    }
}

