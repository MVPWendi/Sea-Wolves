using Assets.Aspects;
using Assets.Components;
using Assets.Scripts.Components;
using System;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Assets
{
    // RPC для передачи данных инвентаря
    public struct InventoryUpdateRpc : IRpcCommand
    {
        public FixedString128Bytes PlayerGUID;
        public NativeArray<Item> Items;
    }

    public struct InventorySyncRequest : IRpcCommand
    {
        public FixedString128Bytes PlayerGUID;
    }
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct ClientInventorySyncSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (req, receive, entity) in SystemAPI.Query<InventoryUpdateRpc, ReceiveRpcCommandRequest>().WithEntityAccess())
            {
                // Обновление буфера на клиенте

                Debug.Log("SYNC 2");
                var entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<PlayerID>());
                var entities = entityQuery.ToEntityArray(Unity.Collections.Allocator.Temp);

                foreach (var ent in entities)
                {
                    Debug.Log("SYNC 3");
                    var playerIDComponent = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<PlayerID>(ent);
                    if (playerIDComponent.Guid == req.PlayerGUID)
                    {
                        var playerAspect = state.EntityManager.GetAspect<PlayerCharacterAspect>(ent);
                        var buffer = playerAspect.GetBuffer(ent, state.EntityManager);
                        var requestBuffer = state.EntityManager.GetBuffer<Item>(entity);
                        buffer.Clear();
                        foreach (var item in requestBuffer)
                        {
                            buffer.Add(item);
                        }
                        Debug.Log("SYNC 4");
                    }
                }             
                ecb.DestroyEntity(entity);
            }
            ecb.Playback(state.EntityManager);
        }
    }

    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerInventorySyncSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            foreach (var (item, entity) in SystemAPI.Query<InventorySyncRequest>().WithEntityAccess())
            {
                Debug.Log("SYNC 1");
                var updateRpc = ecb.CreateEntity();

                Debug.Log(item.PlayerGUID);
                var requestBuffer = state.EntityManager.GetBuffer<Item>(entity);
                Item[] itemDataArray = new Item[requestBuffer.Length];
                for (int i = 0; i < requestBuffer.Length; i++)
                {
                    var item1 = requestBuffer[i];
                    itemDataArray[i] = new Item
                    {
                        Name = item1.Name,
                        Amount = item1.Amount,
                        Cost = item1.Cost,
                        Guid = item1.Guid,
                        Model = item1.Model,
                        // Присвойте значения другим свойствам
                    };
                }
                ecb.AddComponent(updateRpc, new InventoryUpdateRpc
                {
                    PlayerGUID = item.PlayerGUID,
                    Items = new NativeArray<Item>(itemDataArray, Allocator.Temp)
                });                
                Debug.Log("SYNC 12");
                ecb.AddComponent(updateRpc, new SendRpcCommandRequest());
                ecb.DestroyEntity(entity);

                Debug.Log("SYNC 13");
            }
            ecb.Playback(state.EntityManager);
            Debug.Log("SYNC 14");
        }
    }
}