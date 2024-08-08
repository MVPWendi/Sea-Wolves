using Assets;
using Assets.Aspects;
using Assets.Components;
using Assets.Scripts.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.NetCode;
using UnityEditor;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isHidden = false;
    [SerializeField]
    private GameObject Panel;
    private Dictionary<Guid, ItemUI> ItemsUI = new Dictionary<Guid, ItemUI>();
    public ItemUI[] Items;

    public string PlayerGUID;
    void Start()
    {
        Items = gameObject.GetComponentsInChildren<ItemUI>();
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].name = "Item" + i;
            ItemsUI.TryAdd(Items[i].Item.Guid, Items[i]);
        }
    }


    public void InitializeInventory(DynamicBuffer<Item> itemsBuffer)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].SetItem(false);
        }
        for (int i = 0; i < itemsBuffer.Length; i++)
        {
            Items[i].SetItem(true, itemsBuffer[i]);
        }
    }

    public void DropItem(ItemUI item)
    {
        Debug.Log("drop");
        var list = Items.ToList();
        var index = list.IndexOf(item);
        AddRequest(PlayerGUID, index);
    }

    public void AddRequest(string playerGUID, int index)
    {
        // Create an EntityCommandBuffer for temporary allocation
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        // Create a new entity for the request
        var request = ecb.CreateEntity();

        // Add DropRequest component to the entity
        ecb.AddComponent(request, new DropRequest
        {
            ItemIndex = index,
            PlayerGUID = playerGUID
        });

        // Add SendRpcCommandRequest component to the entity
        ecb.AddComponent(request, new SendRpcCommandRequest());

        // Playback the command buffer to apply changes
        ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);

        // Dispose of the command buffer to release resources
        ecb.Dispose();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isHidden = !isHidden;
            Panel.gameObject.SetActive(isHidden);
            
        }
    }


    public class InventoryUIBaker : Baker<InventoryUI>
    {
        public override void Bake(InventoryUI authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, authoring);
        }
    }
}
