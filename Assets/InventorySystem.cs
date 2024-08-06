using Assets.Aspects;
using Assets.Components;
using Assets.Scripts.Components;
using Assets.Scripts.Mono;
using JetBrains.Annotations;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Assets
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct InventorySystem : ISystem
    {
        public static InventorySystem Instance;
        void OnCreate(ref SystemState state)
        {
            Instance = this;
            state.RequireForUpdate<UIPrefabs>();
        }
        void OnUpdate(ref SystemState state)
        {

            Debug.Log("InventorySystem");
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach (var (playerAspect, entity) in SystemAPI.Query<PlayerCharacterAspect>().WithAll<GhostOwnerIsLocal>().WithNone<InventoryUIInitialized>().WithEntityAccess())
            {
                var inventoryPrefab = SystemAPI.ManagedAPI.GetSingleton<UIPrefabs>().InventoryUI;
                var newUI = GameObject.Instantiate(inventoryPrefab);
                newUI.PlayerGUID = playerAspect.ID.ValueRO.Guid.ToString();
                var buffer = playerAspect.GetBuffer(entity, state.EntityManager);
                
                // Create an entity for the InventoryUI and add the InventoryUI component to it
                var uiEntity = ecb.CreateEntity();
                ecb.AddComponent(uiEntity, newUI);
                // Add a reference to the InventoryUI entity to the player entity
                ecb.AddComponent(entity, new InventoryUIReference { InventoryUIEntity = uiEntity });

                newUI.InitializeInventory(buffer);
                ecb.AddComponent(entity, new InventoryUIInitialized());
            }
            foreach (var (item, entity) in SystemAPI.Query<ItemEntity>().WithAll<PickedUpItem>().WithEntityAccess())
            {
                Debug.Log("Picked UP Item: " + entity.Index);
                var inventoryUIRef = state.EntityManager.GetComponentData<InventoryUIReference>(entity);
                var inventoryUI = state.EntityManager.GetComponentObject<InventoryUI>(inventoryUIRef.InventoryUIEntity);
                if (inventoryUI != null)
                {
                    Debug.Log("Found Inventory");
                    // Obtain the inventory and add items to it
                }
            }
            ecb.Playback(state.EntityManager);        
        }


        
    }


    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerInventorySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // Create an EntityCommandBuffer for temporary allocation
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            // Query entities with DropRequest and ReceiveRpcCommandRequest components
            foreach (var (dropRequest, entity) in SystemAPI.Query<DropRequest>().WithEntityAccess())
            {
                // Debug log to verify that request is received
                Debug.Log("Get request!");
                Debug.Log(dropRequest.PlayerGUID);
                Debug.Log(dropRequest.ItemIndex);

                // Create a query to find entities with PlayerID component
                var entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<PlayerID>());
                var entities = entityQuery.ToEntityArray(Unity.Collections.Allocator.Temp);

                foreach (var ent in entities)
                {
                    var playerIDComponent = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<PlayerID>(ent);
                    if (playerIDComponent.Guid == dropRequest.PlayerGUID)
                    {
                        var playerAspect = state.EntityManager.GetAspect<PlayerCharacterAspect>(ent);
                        var playerBuffer = playerAspect.GetBuffer(ent, state.EntityManager);
                        playerBuffer.RemoveAt(dropRequest.ItemIndex);

                        var newEntity = ecb.CreateEntity();
                        ecb.AddComponent(newEntity, new InventorySyncRequest {PlayerGUID = dropRequest.PlayerGUID });
                        var newBuffer = ecb.AddBuffer<Item>(newEntity);
                        for (int i = 0; i < playerBuffer.Length; i++)
                        {
                            newBuffer.Add(playerBuffer[i]);
                        }
                    }
                }

                // Destroy the request entity after processing
                ecb.DestroyEntity(entity);
            }

            // Playback the command buffer to apply changes
            ecb.Playback(state.EntityManager);

            // Dispose of the command buffer to release resources
            ecb.Dispose();
        }

        public void OnCreate(ref SystemState state)
        {

        }
    }
}