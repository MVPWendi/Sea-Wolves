using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
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
        public void ShowHealthBar(Entity entity, EntityManager manager)
        {
            var healthUI = manager.GetComponentObject<CharacterUIS>(entity).HealthBar;
            var newUI = GameObject.Instantiate(healthUI);
            manager.GetComponentObject<CharacterUIS>(entity).HealthBar = newUI;
            newUI.Health = Health.ValueRW.CurrentHealth;
            newUI.MaxHealth = Health.ValueRW.MaxHealth;
            newUI.UpdateHealth();

            AddItem(entity, manager);
        }



        public void AddItem(Entity entity, EntityManager manager)
        {
            var buffer = manager.GetBuffer<Item>(entity);
            buffer.Add(new Item { Amount = 1, Model = Entity.Null, Name = new Unity.Collections.FixedString128Bytes("Item 1") });
            buffer.Add(new Item { Amount = 5, Model = Entity.Null, Name = new Unity.Collections.FixedString128Bytes("Item 2") });
        }
        public void UpdateHealth(Entity entity, EntityManager manager)
        {
            var healthUI = manager.GetComponentObject<CharacterUIS>(entity).HealthBar;
            healthUI.Health = Health.ValueRO.CurrentHealth;
            healthUI.UpdateHealth();
        }
    }
}
