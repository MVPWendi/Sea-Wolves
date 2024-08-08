using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Mono
{
    public class UIPrefabsAuthoring : MonoBehaviour
    {
        public InventoryUI InventoryUI;

        public class Baker : Baker<UIPrefabsAuthoring>
        {
            public override void Bake(UIPrefabsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponentObject(entity, new UIPrefabs { InventoryUIPrefab = authoring.InventoryUI.gameObject });
            }
        }
    }

    public class UIPrefabs : IComponentData
    {
        public GameObject InventoryUIPrefab;
    }
}