﻿using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IDropHandler {
    [SerializeField] private ItemType equipmentType = default;
    [SerializeField] private GameObject equipmentSlotGO = default;

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag.name == "ItemButton") {
            ItemSlot itemSlot = GetComponent<ItemSlot>();

            if (itemSlot.item != DragItem.copyItem.item && DragItem.copyItem.item.itemTypes.Contains(equipmentType)) {
                if (itemSlot.item != null) {
                    Equipments.RemoveEquipment(itemSlot.item);
                    //GetComponentInParent<EquipmentsInventory>().RemoveEquipment(itemSlot.item);
                    eventData.pointerDrag.GetComponentInParent<ItemsInventory>().AddItem(itemSlot.item);
                }

                itemSlot.item = DragItem.copyItem.item;

                GetComponent<EquipmentDisplay>().SetEquipmentDisplay(itemSlot.item);

                if (equipmentSlotGO.GetComponent<DragItem>() == null) {
                    equipmentSlotGO.AddComponent<DragItem>();
                    equipmentSlotGO.GetComponent<DragItem>().originalGO = equipmentSlotGO;
                    RectTransform parent = (RectTransform)GameObject.Find("Inventory").transform;
                    equipmentSlotGO.GetComponent<DragItem>().SetParent(parent);
                }

                if (eventData.pointerDrag.GetComponentInParent<ItemsInventory>().ItemCount(itemSlot.item) == 1) {
                    eventData.pointerDrag.GetComponent<DragItem>().OnEndDrag(eventData);
                    eventData.pointerDrag.GetComponentInParent<ItemsInventory>().RemoveItem(itemSlot.item);
                }
                else
                    eventData.pointerDrag.GetComponentInParent<ItemsInventory>().RemoveItem(itemSlot.item);

                Equipments.AddEquipment(itemSlot.item);

                //GetComponentInParent<EquipmentsInventory>().AddEquipment(itemSlot.item);
            }
        }
    }
}