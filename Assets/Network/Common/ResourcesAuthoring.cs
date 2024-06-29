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

public class ResourcesAuthoring : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject CharacterPrefab;
    public class Baker : Baker<ResourcesAuthoring>
    {
        public override void Bake(ResourcesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ResourceComponent
            {
                PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                CharacterPrefab = GetEntity(authoring.CharacterPrefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}


public struct ResourceComponent : IComponentData
{
    public Entity PlayerPrefab;
    public Entity CharacterPrefab;
}