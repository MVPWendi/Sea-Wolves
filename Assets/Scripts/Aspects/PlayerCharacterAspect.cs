using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Aspects
{
    public readonly partial struct PlayerCharacterAspect : IAspect
    {
        public readonly RefRW<CharacterStatsComponent> Stats;
        public readonly RefRW<HealthComponent> Health;
        public readonly RefRW<InventoryComponent> Inventory;
        public readonly RefRW<FirstPersonCharacterControl> CharControl;
        public readonly RefRW<InteractComponent> Interact;
        public readonly RefRW<LocalTransform> Transform;
        public readonly RefRW<PlayerID> ID;
        public void ShowHealthBar(Entity entity, EntityManager manager)
        {
            var healthUI = manager.GetComponentObject<CharacterUIS>(entity).HealthBar;
            var newUI = GameObject.Instantiate(healthUI);
            manager.GetComponentObject<CharacterUIS>(entity).HealthBar = newUI;
            newUI.Health = Health.ValueRW.CurrentHealth;
            newUI.MaxHealth = Health.ValueRW.MaxHealth;
            newUI.UpdateHealth();

        }

        public DynamicBuffer<Item> GetBuffer(Entity entity, EntityManager manager)
        {
            return manager.GetBuffer<Item>(entity);
        }
        public void UpdateHealth(Entity entity, EntityManager manager)
        {
            var healthUI = manager.GetComponentObject<CharacterUIS>(entity).HealthBar;
            healthUI.Health = Health.ValueRO.CurrentHealth;
            healthUI.UpdateHealth();
        }
    }
}
