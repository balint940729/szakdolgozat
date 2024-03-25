using UnityEngine;
using UnityEngine.EventSystems;

public class DropInventory : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag.CompareTag("Equipment")) {
            //Add back to the inventory
            GameObject invCanvas = eventData.pointerDrag.GetComponentInParent<EquipmentsInventory>().inventoryCanvas;
            GameObject itemCont = invCanvas.GetComponent<InventoryUI>().inventoryContainers.Find(cont => cont.name.Contains("Items"));
            //itemCont.GetComponentInChildren<ItemsInventory>().AddItem(DragItem.copyItem.item);
            ItemsInventory.AddItem(DragItem.copyItem.item);
            Equipments.RemoveEquipment(DragItem.copyItem.item);

            eventData.pointerDrag.GetComponent<EquipmentDisplay>().ResetEquipmentDisplay();
            eventData.pointerDrag.GetComponent<ItemSlot>().item = null;

            eventData.pointerDrag.GetComponent<DragItem>().OnEndDrag(eventData);
            Destroy(eventData.pointerDrag.GetComponent<DragItem>());
        }

        if (eventData.pointerDrag.name.Contains("Teamslot")) {
            Unit[] team = eventData.pointerDrag.GetComponentInParent<Team>().GetTeam();

            //Add back to the inventory
            GameObject invCanvas = eventData.pointerDrag.GetComponentInParent<TeamsInventory>().inventoryCanvas;
            GameObject unitCont = invCanvas.GetComponent<InventoryUI>().inventoryContainers.Find(cont => cont.name.Contains("Units"));
            UnitsInventory.AddUnit(DragUnit.copyUnit.unit);

            //Reset teamslot
            eventData.pointerDrag.GetComponent<TeamSlotDisplay>().ResetTeamSlotDisplay();
            eventData.pointerDrag.GetComponent<UnitItem>().unit = null;

            int.TryParse(eventData.pointerDrag.name.Substring(eventData.pointerDrag.name.Length - 1), out int index);
            team[index] = null;
            eventData.pointerDrag.GetComponent<DragUnit>().OnEndDrag(eventData);
            Destroy(eventData.pointerDrag.GetComponent<DragUnit>());
        }
    }
}