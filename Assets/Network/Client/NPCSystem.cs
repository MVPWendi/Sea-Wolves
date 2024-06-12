using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Network.Client
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct NPCSystem : ISystem
    {
        public void OnCreate(ref SystemState state) 
        {
            state.RequireForUpdate<NPC>();
            state.RequireForUpdate<GameResourcesAuthorings>();
        }


        public void OnUpdate(ref SystemState state) 
        {
            Debug.Log("NPC ");
            GameResourcesAuthorings gameResources = SystemAPI.GetSingleton<GameResourcesAuthorings>();
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach(var (npc, entity) in SystemAPI.Query<NPC>().WithNone<ShowUITag>().WithEntityAccess()) 
            {
                var dialogueUI = SystemAPI.ManagedAPI.GetSingleton<UIS>().DialogueUI;
                var newUI = GameObject.Instantiate(dialogueUI);
                dialogueUI.NPCName.text = npc.Name.ToString();
                ecb.AddComponent(entity, new ShowUITag());
            }
            ecb.Playback(state.EntityManager);
        }


    }

    public struct ShowUITag : IComponentData
    {

    }
}
