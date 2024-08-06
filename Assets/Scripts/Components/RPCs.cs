using Assets.Aspects;
using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Assets.Scripts.Components
{
    public struct DropRequest : IRpcCommand
    {
        public int ItemIndex;
        public FixedString128Bytes PlayerGUID;
    }
}
