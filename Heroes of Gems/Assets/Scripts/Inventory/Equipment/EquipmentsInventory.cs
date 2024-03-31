using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentsInventory : BaseInventory {
    [SerializeField] private List<GameObject> equipmentSlots = default;

    private void Start() {
        FillInventory();
    }

    protected override void FillInventory() {
        equipmentSlots = GameObject.FindGameObjectsWithTag("Equipment").ToList();

        if (!MainMenuScript.isNewGame) {
            foreach (EquipmentsObjectData equipment in Equipments.GetEquipments()) {
                GameObject equipmentSlotGO = equipmentSlots.First(e => e.GetComponent<DropItem>().GetEquipmentSlotType() == equipment.equipmentSlot);
                equipmentSlotGO.GetComponent<ItemSlot>().item = equipment.item;
                equipmentSlotGO.GetComponent<EquipmentDisplay>().SetEquipmentDisplay(equipment.item);
                equipmentSlotGO.GetComponent<DropItem>().AddDragItem();
            }
        }
    }
}