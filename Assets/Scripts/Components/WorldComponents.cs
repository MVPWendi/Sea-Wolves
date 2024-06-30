using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Assets.Components
{
    public struct ItemEntity : IComponentData
    {
        public Guid Guid;
    }

    public struct Item : IBufferElementData
    {
        public Guid Guid;
        public Entity Model;
        public FixedString128Bytes Name;
        public int Amount;
    }
    public struct GoInGameRequest : IRpcCommand { }
}
