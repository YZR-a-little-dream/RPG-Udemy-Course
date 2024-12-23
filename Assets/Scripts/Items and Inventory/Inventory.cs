using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemData> startingItem;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment,InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData,InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData,InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParant;
    [SerializeField] private Transform staashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items Cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    //确保第一次使用技能不进入冷却
    //Make sure the first use of the skill does not enter the cooldown
    private float flaskCooldown;
    private float armorCooldown;

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParant.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = staashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItem();
    }

    private void AddStartingItem()
    {
        for (int i = 0; i < startingItem.Count; i++)
        {
            AddItem(startingItem[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(_item);

        ItemData_Equipment oldEquipment = null;

        //判断当前装备和之前装备类型是否一致
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        //增加装备属性
        newEquipment.AddModifier();
        //移除在装备仓库中的武器
        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            //判断当前装备和之前装备类型是否一致
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        //update info of stats in character UI
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if(_item.itemtype == Itemtype.Equipment && CanAddItem())
            AddToInventory(_item);
        else if(_item.itemtype == Itemtype.Material)
            AddToStash(_item);
            
        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item,out InventoryItem value))
        {
            if(value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.removeStack();
        }

        if(stashDictionary.TryGetValue(_item,out InventoryItem stashValue))
        {
            if(stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.removeStack();
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if(inventory.Count >= inventoryItemSlot.Length)
        {
            return false;
        }
        return true;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.L))
        {
            ItemData newItem = inventory[inventory.Count - 1].data;

            RemoveItem(newItem);
        }
    }

    public bool CanCraft(ItemData _itemToCraft,List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if(stashDictionary.TryGetValue(_requiredMaterials[i].data,out InventoryItem stashValue))
            {
                //The material is less than the need to realistically
                if(stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    //add this to used material
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Here is your item " + _itemToCraft.name);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        //判断当前装备和之前装备类型是否一致
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        //if can use   //cooldown
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if(currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.ItemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log($"flask on cooldown + {currentFlask.ItemCooldown - (Time.time -lastTimeUsedFlask)}");
        }
        
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);

        if(Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown= currentArmor.ItemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor on cooldown");
        return false;
    }
}
