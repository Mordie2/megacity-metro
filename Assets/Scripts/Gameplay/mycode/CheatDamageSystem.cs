using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Unity.MegacityMetro.Gameplay
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial struct CheatDamageSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnableCheatDamageSystem>();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (input, health, entity) in SystemAPI.Query<RefRO<PlayerVehicleInput>, RefRW<VehicleHealth>>().WithEntityAccess())
            {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                if (input.ValueRO.Cheat_1)
                {
                    health.ValueRW.Value -= 10f;
                    Debug.Log($"[CHEAT] Damage applied. New health: {health.ValueRW.Value}");
                }
#endif
            }
        }
    }

    public struct EnableCheatDamageSystem : IComponentData { }
}
