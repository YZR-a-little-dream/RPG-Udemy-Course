using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
   [Header("Player's drop")]
   [SerializeField] private float chanceToLoseItems;
   [SerializeField] private float chanceToMaterials;

    public override void GenerateDrop()
    {
        //list of equipment
        Inventory inventory = Inventory.instance;
        
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLose = new List<InventoryItem>();

        //foreach item we gonna check if should loose item
        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if(Random.Range(0,100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }
        
        foreach (InventoryItem item in inventory.GetStashList())
        {
            if(Random.Range(0,100) <= chanceToMaterials)
            {
                DropItem(item.data);
                materialsToLose.Add(item);
            }
        }

        for (int i = 0; i < materialsToLose.Count; i++)
        {
            inventory.RemoveItem(materialsToLose[i].data);
        }
    }
}
