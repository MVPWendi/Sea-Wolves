using Assets.Aspects;
using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Scripts.Common
{
    public static class Utils
    {
        public static Entity GetPlayerEntity(string playerGUID, EntityManager manager)
        {
            var entityQuery = manager.CreateEntityQuery(ComponentType.ReadOnly<PlayerID>());
            var entities = entityQuery.ToEntityArray(Unity.Collections.Allocator.Temp);
            foreach (var ent in entities)
            {
                var playerIDComponent = manager.GetComponentData<PlayerID>(ent);
                if (playerGUID == playerIDComponent.Guid)
                {
                    return ent;
                }
            }
            return Entity.Null;
        }

        public static Entity GetPlayerEntity(FixedString128Bytes playerGUID, EntityManager manager) => GetPlayerEntity(playerGUID.ToString(), manager);
        
    }
}
