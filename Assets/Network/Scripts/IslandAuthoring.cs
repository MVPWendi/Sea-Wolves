using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class IslandAuthoring : MonoBehaviour
    {
        public string Name;
        public int Test;

    }


    public partial class Baker : Baker<IslandAuthoring>
    {
        public override void Bake(IslandAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Island { Name = new FixedString128Bytes(authoring.Name), Test  = authoring.Test});
        }
    }
}

public struct Island : IComponentData
{
    public FixedString128Bytes Name;
    public int Test;
}
