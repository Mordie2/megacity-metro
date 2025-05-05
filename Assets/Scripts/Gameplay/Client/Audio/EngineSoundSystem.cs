using Unity.Entities;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Unity.Collections;
using System.Collections.Generic;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.LocalSimulation)]
public partial class EngineSoundSystem : SystemBase
{
    private EntityQuery _engineQuery;
    private Dictionary<Entity, EventInstance> _activeEngines;

    protected override void OnCreate()
    {
        _engineQuery = GetEntityQuery(ComponentType.ReadOnly<EngineSoundRequest>());
        _activeEngines = new Dictionary<Entity, EventInstance>();
    }

    protected override void OnUpdate()
    {
        var requests = _engineQuery.ToComponentDataArray<EngineSoundRequest>(Allocator.Temp);

        foreach (var request in requests)
        {
            var entity = request.Entity;

            if (request.IsAlive)
            {
                if (!_activeEngines.TryGetValue(entity, out var instance))
                {
                    instance = AudioManager.instance.CreateInstance(FMODEvents.instance.ShipEngine);
                    instance.set3DAttributes(RuntimeUtils.To3DAttributes(request.Position));
                    instance.start();
                    _activeEngines.Add(entity, instance);
                }
                else
                {
                    instance.set3DAttributes(RuntimeUtils.To3DAttributes(request.Position));
                }

                _activeEngines[entity].setParameterByName("Idle", request.IdleFactor);
                _activeEngines[entity].setParameterByName("Damage", request.DamageFactor);
            }
            else
            {
                if (_activeEngines.TryGetValue(entity, out var instance))
                {
                    instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    instance.release();
                    _activeEngines.Remove(entity);

                    Debug.Log($"[EngineSoundSystem] Stopped engine sound for Entity {entity.Index}");
                }
            }
        }

        if (!_engineQuery.IsEmptyIgnoreFilter)
        {
            EntityManager.RemoveComponent<EngineSoundRequest>(_engineQuery);
        }
    }

    protected override void OnDestroy()
    {
        foreach (var kvp in _activeEngines)
        {
            kvp.Value.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            kvp.Value.release();
        }
        _activeEngines.Clear();
    }
}
