using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Network.Client
{
   // [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct NPCSystem : ISystem
    {
        public void OnCreate(ref SystemState state) 
        {
            state.RequireForUpdate<NPC>();
            state.RequireForUpdate<GameResourcesAuthorings>();
        }


        public void OnUpdate(ref SystemState state) 
        {
            GameResourcesAuthorings gameResources = SystemAPI.GetSingleton<GameResourcesAuthorings>();
         foreach(var (npc, entity) in SystemAPI.Query<NPC>().WithNone<NPCDialogueRefering>().WithEntityAccess()) 
            {
                var ui = gameResources.NPC
            }
            
        }


    }
}
