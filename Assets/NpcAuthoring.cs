using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class NpcAuthoring : MonoBehaviour
{
    public string Name;

    public class Baker : Baker<NpcAuthoring>
    {
        
        public override void Bake(NpcAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new NPC
            {
                Name = new FixedString128Bytes(authoring.Name)
            });
                

            
        }
    }
}

public struct NPC : IComponentData
{
    public FixedString128Bytes Name;
}
