using Unity.Entities;
using UnityEngine;
using FMODUnity;

public class VehicleAudioManager : MonoBehaviour
{
    private EntityManager entityManager;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Update()
    {
        var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<LaserSoundRequest>());
        var requests = query.ToComponentDataArray<LaserSoundRequest>(Unity.Collections.Allocator.Temp);

        foreach (var request in requests)
        {
            if (request.IsFiring)
            {
                RuntimeManager.PlayOneShot(FMODEvents.instance.ShipLaser, request.Position);
                Debug.Log($"[LaserSound] One-shot fired at {request.Position}");
            }
        }

        entityManager.RemoveComponent<LaserSoundRequest>(query);
    }
}
