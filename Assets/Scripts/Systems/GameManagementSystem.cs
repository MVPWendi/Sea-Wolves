﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Network.Common
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct GameManagementSystemClient : ISystem
    {
        void OnCreate(ref SystemState state)
        {
        }
        void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach (var (character, owningPlayer, ghostOwner, entity) in SystemAPI.Query<FirstPersonCharacterComponent, OwningPlayer, GhostOwner>().WithAll<GhostOwnerIsLocal>().WithNone<CharacterInitialized>().WithEntityAccess())
            {
                // Make camera follow character's view
                ecb.AddComponent(character.ViewEntity, new MainEntityCamera { });

                // Make local character meshes rendering be shadow-only
                BufferLookup<Child> childBufferLookup = SystemAPI.GetBufferLookup<Child>();
                MiscUtilities.SetShadowModeInHierarchy(state.EntityManager, ecb, entity, ref childBufferLookup, UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly);

                // Mark initialized
                ecb.AddComponent<CharacterInitialized>(entity);
            }
            ecb.Playback(state.EntityManager);
        }
    }
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct GameManagementSystemServer : ISystem
    {
        void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ResourceComponent>();
        }
        void OnUpdate(ref SystemState state)
        {
            var gameResources = SystemAPI.GetSingleton<ResourceComponent>();
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach (var (spawnRequest, entity) in SystemAPI.Query<RefRO<CharacterSpawnRequest>>().WithEntityAccess())
            {
                // Spawn character and player ghost prefabs
                Entity characterEntity = ecb.Instantiate(gameResources.CharacterPrefab);
                Entity playerEntity = ecb.Instantiate(gameResources.PlayerPrefab);

                // Add spawned prefabs to the connection entity's linked entities, so they get destroyed along with it
                ecb.AppendToBuffer(spawnRequest.ValueRO.ForConnection, new LinkedEntityGroup { Value = characterEntity });
                ecb.AppendToBuffer(spawnRequest.ValueRO.ForConnection, new LinkedEntityGroup { Value = playerEntity });

                // Setup the owners of the ghost prefabs (which are all owner-predicted) 
                // The owner is the client connection that sent the join request
                int clientConnectionId = SystemAPI.GetComponent<NetworkId>(spawnRequest.ValueRO.ForConnection).Value;
                ecb.SetComponent(characterEntity, new GhostOwner { NetworkId = clientConnectionId });
                ecb.SetComponent(playerEntity, new GhostOwner { NetworkId = clientConnectionId });

                // Setup links between the prefabs
                FirstPersonPlayer player = SystemAPI.GetComponent<FirstPersonPlayer>(gameResources.PlayerPrefab);
                player.ControlledCharacter = characterEntity;
                ecb.SetComponent(playerEntity, player);

                // Place character at a random point around world origin
                ecb.SetComponent(characterEntity, LocalTransform.FromPosition(new float3(0, 5, 0)));

                ecb.AddComponent<NetworkStreamInGame>(spawnRequest.ValueRO.ForConnection);
                ecb.DestroyEntity(entity);
            }

            ecb.Playback(state.EntityManager);
        }
    }

    public struct CharacterSpawnRequest : IComponentData
    {
        public Entity ForConnection;
    }
}