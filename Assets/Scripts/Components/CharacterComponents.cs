using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Assets.Components
{
    [GhostComponent]
    public struct HealthComponent : IComponentData
    {
        [GhostField] public float MaxHealth;
        [GhostField] public float CurrentHealth;
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

    
}
