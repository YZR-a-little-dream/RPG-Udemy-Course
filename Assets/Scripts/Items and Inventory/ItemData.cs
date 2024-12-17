using UnityEngine;

public enum Itemtype
{
   Material,
   Equipment,
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
   public Itemtype itemtype;
   public string itemName;
   public Sprite icon;
   
   [Range(0,100)]
   public float dropChance;      //µôÂä¸ÅÂÊ
}
