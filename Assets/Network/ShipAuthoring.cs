using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

namespace Assets.Network
{

    class ShipBaker : Baker<Ship>
    {
        public override void Bake(Ship authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShipComponent
            {
                GameObject = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic),
                Name = "Test"
            });
            AddComponent(entity, new ShipInputs { 
            MoveInput = new float2(),
            SailPressed = new InputEvent()});
        }
    }
    [GhostComponent]
    [Serializable]
    public struct ShipComponent : IInputComponentData
    {
        [GhostField]
        public Entity GameObject;
        public FixedString128Bytes Name;
        public InputEvent SailPressed;
    }

    [Serializable]
    public struct ShipInputs : IInputComponentData
    {
        public float2 MoveInput;
        public InputEvent SailPressed;
    }
}
