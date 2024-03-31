using UnityEngine;

[System.Serializable]
public class EquipmentsObjectData {
    [SerializeField] public Item item;
    [SerializeField] public ItemType equipmentSlot;

    public EquipmentsObjectData(Item item, ItemType equipmentSlot) {
        this.item = item;
        this.equipmentSlot = equipmentSlot;
    }
}