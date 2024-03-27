using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Weapon, Helmet, BodyArmor, Cape, Gloves, Pants, Boots, OffHand, Trinkets, Other };

[CreateAssetMenu]
[System.Serializable]
public class Item : Lootable {
    public string itemName;
    public Sprite itemIcon;
    public List<ItemType> itemTypes;
    public BaseBonus bonus;
}