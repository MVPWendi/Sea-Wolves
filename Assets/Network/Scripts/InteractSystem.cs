using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct InteractSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkTime>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.Enabled = false;
        }

        public void OnUpdate(ref SystemState state)
        {

            var networkTime = SystemAPI.GetSingleton<NetworkTime>();
            if (networkTime.ServerTick.IsValid && !networkTime.IsFirstTimeFullyPredictingTick) return;
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            foreach (var (player, input, entity) in SystemAPI.Query<RefRW<FirstPersonPlayer>, FirstPersonPlayerInputs>().WithAll<Simulate>().WithEntityAccess())
            {
                if (!input.InteractPressed.IsSet)
                {

                    return;
                }
                var interact = SystemAPI.GetComponent<InteractComponent>(player.ValueRO.ControlledCharacter);
                FirstPersonCharacterComponent character = SystemAPI.GetComponent<FirstPersonCharacterComponent>(player.ValueRO.ControlledCharacter);
                Debug.Log("interact on update true");
                // Получаем PhysicsWorldSingleton
                PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

                // Получаем положение и ориентацию камеры (дочернего объекта)
                var cameraTransform = SystemAPI.GetComponent<LocalToWorld>(character.ViewEntity);

                // Получаем позицию камеры
                var cameraPosition = cameraTransform.Position;

                // Получаем направление камеры (переднее направление из матрицы LocalToWorld)
                var cameraForward = cameraTransform.Forward;

                // Вычисляем конечную точку рейкаста
                var rayEnd = cameraPosition + cameraForward * interact.Distance;

                // Создаем RaycastInput
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = cameraPosition,
                    End = rayEnd,
                    Filter = new CollisionFilter
                    {
                        CollidesWith = (uint)CollisionLayers.Interactable,
                        BelongsTo = (uint)CollisionLayers.Player
                    }
                };

                // Выполняем рейкаст
                if (physicsWorld.CastRay(raycastInput, out var hit))
                {
                    
                    if(state.EntityManager.HasComponent<NPCComponent>(hit.Entity))
                    {
                        Debug.Log("hit: " + hit.Entity.Index);
                    }
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
    public enum CollisionLayers
    {
        Interactable = 1 << 6,
        Player = 1 << 7
    }
    public struct InteractComponent : IComponentData
    {
        public Unity.Physics.RaycastHit InteractedEntity;
        public float Distance;
    }
}
