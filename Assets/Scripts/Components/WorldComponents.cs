using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEditor;

namespace Assets.Components
{
    public struct ItemEntity : IComponentData
    {
        public Guid Guid;
        public FixedString128Bytes Name;
        public int Amount;
        public float Cost;
    }

    public struct Item : IBufferElementData
    {
        public Guid Guid;
        public Entity Model;
        public FixedString128Bytes Name;
        public int Amount;
        public float Cost;
    }
    public struct GoInGameRequest : IRpcCommand { }

    [GhostComponent]
    public struct PickedUpItem : IComponentData
    {
        [GhostField]
        public Entity Player;
    }
}
