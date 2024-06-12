using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class MovingPlatformAuthoring : MonoBehaviour
{
    public MovingPlatform MovingPlatform;

    public class Baker : Baker<MovingPlatformAuthoring>
    {
        public override void Bake(MovingPlatformAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, authoring.MovingPlatform);
        }
    }
}
[Serializable]
public struct MovingPlatform : IComponentData
{
    public float3 TranslationAxis;
    public float TranslationAmplitude;
    public float TranslationSpeed;
    public float3 RotationAxis;
    public float RotationSpeed;

    [HideInInspector]
    public bool IsInitialized;
    [HideInInspector]
    public float3 OriginalPosition;
    [HideInInspector]
    public quaternion OriginalRotation;
}

[UpdateInGroup(typeof(BeforePhysicsSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial class MovingPlatformSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float invDeltaTime = 1f / deltaTime;
        float time = (float)World.Time.ElapsedTime;

        foreach (var (movingPlatform, physicsVelocity, physicsMass, localTransform, entity) in SystemAPI.Query<RefRW<MovingPlatform>, RefRW<PhysicsVelocity>, PhysicsMass, LocalTransform>().WithEntityAccess())
        {
            Debug.Log("Move system");
            if (!movingPlatform.ValueRW.IsInitialized)
            {
                // Remember initial pos/rot, because our calculations depend on them
                movingPlatform.ValueRW.OriginalPosition = localTransform.Position;
                movingPlatform.ValueRW.OriginalRotation = localTransform.Rotation;
                movingPlatform.ValueRW.IsInitialized = true;
            }

            float3 targetPos = movingPlatform.ValueRW.OriginalPosition + (math.normalizesafe(movingPlatform.ValueRW.TranslationAxis) * math.sin(time * movingPlatform.ValueRW.TranslationSpeed) * movingPlatform.ValueRW.TranslationAmplitude);
            quaternion rotationFromMovement = quaternion.Euler(math.normalizesafe(movingPlatform.ValueRW.RotationAxis) * movingPlatform.ValueRW.RotationSpeed * time);
            quaternion targetRot = math.mul(rotationFromMovement, movingPlatform.ValueRW.OriginalRotation);
            Debug.Log(entity.Index);
            Debug.Log(targetPos);
            // Move with velocity
            physicsVelocity.ValueRW = PhysicsVelocity.CalculateVelocityToTarget(in physicsMass, localTransform.Position, localTransform.Rotation, new RigidTransform(targetRot, targetPos), invDeltaTime);
        }
    }
}