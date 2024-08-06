using Assets.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ItemAuthoring : MonoBehaviour
{
    public string Name;
    public int Amount;
    public Guid Guid;
    public float Cost;



    public class Baker : Baker<ItemAuthoring>
    {
        public override void Bake(ItemAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ItemEntity
            {
                Amount = authoring.Amount,
                Guid = authoring.Guid,
                Cost = authoring.Cost,
                Name = authoring.Name,
            });
        }
    }
}
