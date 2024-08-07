using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEditor;
using UnityEngine;

namespace Assets.Components
{
    [GhostComponent]
    public struct HealthComponent : IComponentData
    {
        [GhostField] public float MaxHealth;
        [GhostField] public float CurrentHealth;
    }

    [GhostComponent]
    public struct PlayerID : IComponentData
    {
        [GhostField] public FixedString128Bytes Guid;
        [GhostField] public FixedString128Bytes Name;
    }
    public class HealthUIComponent : IComponentData
    {
        public HealthBarUI HealthUI;
    }
    public struct CharacterStatsComponent : IComponentData
    {
        public int Strength;
        public int Agility;
        public int Intellegence;
        public int Charisma;
        public int Industry;
    }

    public struct InventoryComponent : IComponentData
    {
        
    }


    public struct ShowHealthBarTag : IComponentData
    {

    }

    public struct InventoryUIInitialized : IComponentData
    {

    }

    public struct InventoryUIReference : IComponentData
    {
        public Entity InventoryUIEntity;
    }
}
