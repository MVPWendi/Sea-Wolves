using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEngine;

[Serializable]
public struct AIController : IComponentData
{
    public float DetectionDistance;
}

public class AIControllerAuthoring : MonoBehaviour
{
    public AIController AIController;

    class Baker : Baker<AIControllerAuthoring>
    {
        public override void Bake(AIControllerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, authoring.AIController);
        }
    }
}

public partial class AIControllerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        PhysicsWorld physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
        NativeList<DistanceHit> distanceHits = new NativeList<DistanceHit>(Allocator.TempJob);
            foreach (var (characterControl, aiController, localTransform) in SystemAPI.Query<RefRW<FirstPersonCharacterControl>, AIController, LocalTransform>())
        {
            
            // Clear our detected hits list between each use
            distanceHits.Clear();

            // Create a hit collector for the detection hits
            AllHitsCollector<DistanceHit> hitsCollector = new AllHitsCollector<DistanceHit>(aiController.DetectionDistance, ref distanceHits);

            // Detect hits that are within the detection range of the AI character
            PointDistanceInput distInput = new PointDistanceInput
            {
                Position = localTransform.Position,
                MaxDistance = aiController.DetectionDistance,
            };
            physicsWorld.CalculateDistance(distInput, ref hitsCollector);

            // Iterate on all detected hits to try to find a human-controlled character...
            Entity selectedTarget = Entity.Null;
            for (int i = 0; i < hitsCollector.NumHits; i++)
            {
                Entity hitEntity = distanceHits[i].Entity;

                // If it has a character component but no AIController component, that means it's a human player character
                if (SystemAPI.HasComponent<FirstPersonCharacterComponent>(hitEntity) && !SystemAPI.HasComponent<AIController>(hitEntity))
                {
                    selectedTarget = hitEntity;
                    break; // early out
                }
            }

            // In the character control component, set a movement vector that will make the ai character move towards the selected target
            
            if (selectedTarget != Entity.Null)
            {
                characterControl.ValueRW.MoveVector = math.normalizesafe(SystemAPI.GetComponent<LocalTransform>(selectedTarget).Position - localTransform.Position);
            }
            else
            {
                characterControl.ValueRW.MoveVector = float3.zero;
            }
        }
    }
}