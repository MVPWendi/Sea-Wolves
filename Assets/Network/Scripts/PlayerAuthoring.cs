using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public struct Player
    {
        public float MaxHealth;
        public float Health;
    }

    public struct TakeDamage : IComponentData
    {
        public float Damage;
    }
    //public partial class HealthSystem : SystemBase
    //{
    //    protected override void OnUpdate()
    //    {
    //        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
    //        foreach (var healthComp in SystemAPI.Query<RefRO<FirstPersonPlayer>>())
    //        {
    //            Debug.Log("Health: " + healthComp.ValueRO.Player.Health);
    //        }
            
    //        foreach (var (healthComp, damageComp, entity)in SystemAPI.Query<RefRW<FirstPersonPlayer>, TakeDamage>().WithEntityAccess())
    //        {
    //            Debug.Log("Health222");
    //            healthComp.ValueRW.Player.Health -= damageComp.Damage;
    //            ecb.RemoveComponent(entity, typeof(TakeDamage));
    //        }
    //        ecb.Playback(World.EntityManager);
    //        ecb.Dispose();
            
    //    }
    //}
}
