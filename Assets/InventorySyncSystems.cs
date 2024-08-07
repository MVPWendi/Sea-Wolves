using Assets.Aspects;
using Assets.Components;
using Assets.Scripts.Common;
using Assets.Scripts.Components;
using System;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Item = Assets.Components.Item;

namespace Assets
{
    // RPC для передачи данных инвентаря
    public struct InventoryUpdateRpc : IRpcCommand
    {

        public FixedString128Bytes PlayerGUID;
        public int RemoveIndex;
    }
    
    public struct InventorySyncRequest : IComponentData
    {
        public FixedString128Bytes PlayerGUID;
        public int RemoveIndex;
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
            foreach (var (req, receiveRPC, entity) in SystemAPI.Query<InventoryUpdateRpc, ReceiveRpcCommandRequest>().WithEntityAccess())
            {
                // Обновление буфера на клиенте
                var playerEntity = Utils.GetPlayerEntity(req.PlayerGUID, state.EntityManager);
                var buffer = state.EntityManager.GetBuffer<Item>(playerEntity);
                Debug.Log("Client message: buffer length BEFORE: " + buffer.Length);
                buffer.RemoveAt(req.RemoveIndex);
                //buffer.Add(new Item { Amount = 1, Cost = 1, Guid = new System.Guid(), Name = new Unity.Collections.FixedString128Bytes("Аптечка") });
                Debug.Log("Client message: buffer length AFTER: " + buffer.Length);
                var inventoryUIRef = state.EntityManager.GetComponentData<InventoryUIReference>(playerEntity);
                var inventoryUI = state.EntityManager.GetComponentObject<InventoryUI>(inventoryUIRef.InventoryUIEntity);
                inventoryUI.InitializeInventory(buffer);
                // Вот тут
                ecb.DestroyEntity(entity);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
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
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (item, entity) in SystemAPI.Query<InventorySyncRequest>().WithEntityAccess())
            {
                var updateRpc = ecb.CreateEntity();
                ecb.AddComponent(updateRpc, new InventoryUpdateRpc
                {
                    PlayerGUID = item.PlayerGUID,
                    RemoveIndex = item.RemoveIndex,
                });
                var playerEntity = Utils.GetPlayerEntity(item.PlayerGUID, state.EntityManager);
                var buffer = state.EntityManager.GetBuffer<Item>(playerEntity);
                Debug.Log("Server message: PLAYER ENTITY INDEX: " + playerEntity.Index);
                Debug.Log("Server message: buffer length BEFORE: " + buffer.Length);
                buffer.RemoveAt(item.RemoveIndex);
                //buffer.Add(new Item { Amount = 1, Cost = 1, Guid = new System.Guid(), Name = new Unity.Collections.FixedString128Bytes("Аптечка") });
                Debug.Log("Server message: buffer length AFTER: " + buffer.Length);
                ecb.AddComponent(updateRpc, new SendRpcCommandRequest());
                ecb.DestroyEntity(entity);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}