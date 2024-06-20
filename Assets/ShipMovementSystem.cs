using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace Assets
{

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ShipMovementSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkTime>();
        }
        public void OnUpdate(ref SystemState state)
        {
            Debug.Log("ShipMovementSystem");
            foreach (var (shipComponent, localTtransform, physicsVelocity) in SystemAPI.Query<RefRW<ShipComponent>, RefRW<LocalTransform>, RefRW<PhysicsVelocity>>())
            {
                float speed = shipComponent.ValueRO.Speed;
                if (shipComponent.ValueRO.ShouldGoForward)
                {
                    speed = math.clamp(speed += shipComponent.ValueRO.AccelerationSpeed * SystemAPI.Time.DeltaTime, 0, shipComponent.ValueRO.MaxSpeed);
                }
                else
                {
                    speed = math.clamp(speed -= shipComponent.ValueRO.AccelerationSpeed /2 * SystemAPI.Time.DeltaTime, 0, shipComponent.ValueRO.MaxSpeed);
                }

                shipComponent.ValueRW.Speed = speed;
                if (shipComponent.ValueRO.ShouldRotate)
                {
                    if (shipComponent.ValueRO.RotateRight)
                    {
                        physicsVelocity.ValueRW.Angular = new float3(0, shipComponent.ValueRO.RotationSpeed * SystemAPI.Time.DeltaTime, 0);
                    }
                    else
                    {
                        physicsVelocity.ValueRW.Angular = new float3(0, -shipComponent.ValueRO.RotationSpeed * SystemAPI.Time.DeltaTime, 0);
                    }
                }
                else
                {
                    physicsVelocity.ValueRW.Angular = new float3(0, 0, 0);
                }







                float3 moveVector = localTtransform.ValueRO.Forward() * shipComponent.ValueRW.Speed;
                physicsVelocity.ValueRW.Linear = moveVector;
            }
        }
    }
}
