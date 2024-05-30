using Assets.Scripts;
using NUnit.Framework;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.NetworkV2
{
    [BurstCompile]
    public partial struct TestSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            ProcessTestComponent(ref state);
        }

        private void ProcessTestComponent(ref SystemState state)
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach (RefRW<TestCompData> testComp in SystemAPI.Query<RefRW<TestCompData>>().WithNone<SpawnedTestComp>())
            {
                var Entity = commandBuffer.Instantiate(testComp.ValueRO.Prefab);
                commandBuffer.SetComponent(Entity, LocalTransform.FromPosition(float3.zero));
                commandBuffer.SetComponent(Entity, new TestCompData { Color = testComp.ValueRO.Color, Name = "HI", Prefab = testComp.ValueRO.Prefab });
                commandBuffer.AddComponent(Entity, new SpawnedTestComp());
            }
            commandBuffer.Playback(state.EntityManager);
            commandBuffer.Dispose();
        }
    }

    public struct SpawnedTestComp : IComponentData
    {

    }



}
