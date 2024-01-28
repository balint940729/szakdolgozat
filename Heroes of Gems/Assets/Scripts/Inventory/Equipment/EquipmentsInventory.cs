using System.Collections.Generic;
using UnityEngine;

public class EquipmentsInventory : BaseInventory {
    [SerializeField] private List<Item> equipments = new List<Item>();

    private void Start() {
        FillInventory();
    }

    protected override void FillInventory() {
        //throw new System.NotImplementedException();
    }

    public void AddEquipment(Item item) {
        equipments.Add(item);
    }

    public void RemoveEquipment(Item item) {
        equipments.Remove(item);
    }
}