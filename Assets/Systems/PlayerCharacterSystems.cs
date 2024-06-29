using Assets.Aspects;
using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Assets.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct PlayerCharacterSystems : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            //foreach (var (aspect, entity) in SystemAPI.Query<PlayerCharacterAspect>().WithAll<CharacterUIS>().WithNone<ShowHealthBarTag>().WithEntityAccess())
            //{
            //    if (state.World.IsClient())
            //    {
            //        aspect.ShowHealthBar(entity, state.EntityManager);
            //        ecb.AddComponent(entity, new ShowHealthBarTag());
            //    }       
            //}
            //foreach (var (aspect, entity) in SystemAPI.Query<PlayerCharacterAspect>().WithAll<CharacterUIS>().WithAll<ShowHealthBarTag>().WithEntityAccess())
            //{
            //    if (state.World.IsClient())
            //    {
            //        aspect.UpdateHealth(entity, state.EntityManager);
            //    }
            //}
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
