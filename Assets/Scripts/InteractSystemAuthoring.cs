using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    [BurstCompile]
    public partial struct InteractSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            
            state.RequireForUpdate<NetworkTime>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            return;
            foreach (var (interact, entity) in SystemAPI.Query<RefRW<InteractComponent>>().WithEntityAccess())
            {
                if (interact.ValueRO.InteractPressed)
                {
                    Debug.Log("interact on update true");
                    interact.ValueRW.InteractPressed = false;
                }
                // Получаем PhysicsWorldSingleton
                PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

                // Получаем положение и ориентацию камеры (дочернего объекта)
                var cameraTransform = SystemAPI.GetComponent<LocalToWorld>(interact.ValueRO.ViewPoint);

                // Получаем позицию камеры
                var cameraPosition = cameraTransform.Position;

                // Получаем направление камеры (переднее направление из матрицы LocalToWorld)
                var cameraForward = cameraTransform.Forward;

                // Вычисляем конечную точку рейкаста
                var rayEnd = cameraPosition + cameraForward * interact.ValueRO.Distance;
                // Визуализация луча для отладки
                Debug.DrawLine(cameraPosition, rayEnd, Color.red);
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
                    interact.ValueRW.InteractedEntity = new Unity.Physics.RaycastHit
                    {
                        Entity = hit.Entity,
                        Position = hit.Position
                    };

                    var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
                    // Если попали в объект с компонентом Island, выводим его имя
                    if (SystemAPI.HasComponent<Island>(hit.Entity))
                    {
                        Debug.Log(SystemAPI.GetComponent<Island>(hit.Entity).Name);
                    }
                }
            }
        }

    }
    public struct InteractComponent : IComponentData
    {
        public Unity.Physics.RaycastHit InteractedEntity;
        public Entity ViewPoint;
        public Entity ViewDestination;
        public float Distance;
        public bool InteractPressed; // Новое поле для состояния кнопки F
    }


    public enum CollisionLayers
    {
        Interactable = 1 << 6,
        Player = 1 << 7
    }
}
