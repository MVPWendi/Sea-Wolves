using Assets.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    public GameObject PlayerGhost;
    public GameObject CharacterGhost;
    public class Baker : Baker<GameResources>
    {
        public override void Bake(GameResources authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new GameResourcesAuthorings
            {
                PlayerGhost = GetEntity(authoring.PlayerGhost, TransformUsageFlags.Dynamic),
                CharacterGhost = GetEntity(authoring.CharacterGhost, TransformUsageFlags.Dynamic),
            });
        }
    }
}