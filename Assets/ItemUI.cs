using Assets.Components;
using Assets.Scripts.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ItemUI : MonoBehaviour
{

    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text Cost;
    [SerializeField] private TMP_Text Amount;
    [SerializeField] private Button Drop;
    [SerializeField] private Button Equip;
    [SerializeField] private Image ItemIcon;
    [SerializeField] private GameObject Container;
    private InventoryUI inventoryUI;
    public bool HaveItem;
    public Item Item { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = gameObject.GetComponentInParent<InventoryUI>();
        Equip.onClick.AddListener(() => OnEquipClick());
        Drop.onClick.AddListener(() => OnDropClick());
    }

    private void OnDropClick()
    {
        inventoryUI.DropItem(this);
    }

    private void OnEquipClick()
    {
        Debug.Log("equip");
    }
    public void SetItem(bool haveItem, Item newItem = new Item())
    {
        Item = newItem;
        HaveItem = haveItem;
        FillItem();
    }
    private void FillItem()
    {
        if(!HaveItem)
        {
            Container.SetActive(false);
            return;
        }
        Container.SetActive(true);
        Name.text = Item.Name.ToString();
        Cost.text = Item.Cost.ToString() + " RUB";
        Amount.text = Item.Amount.ToString() + "רע";

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
