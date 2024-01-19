using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag.name.Contains("Teamslot")) {
            Unit[] team = eventData.pointerDrag.GetComponentInParent<Team>().team;

            //Add back to the inventory
            GameObject invCanvas = eventData.pointerDrag.GetComponentInParent<TeamsInventory>().inventoryCanvas;
            GameObject unitCont = invCanvas.GetComponent<InventoryUI>().inventoryContainers.Find(cont => cont.name.Contains("Units"));
            unitCont.GetComponentInChildren<UnitsInventory>().AddUnit(DragUnit.copyUnit.unit);

            //Reset teamslot
            eventData.pointerDrag.GetComponent<TeamSlotDisplay>().ResetTeamSlotDisplay();
            eventData.pointerDrag.GetComponent<UnitItem>().unit = null;

            int.TryParse(eventData.pointerDrag.name.Substring(eventData.pointerDrag.name.Length), out int index);
            team[index] = null;
            eventData.pointerDrag.GetComponent<DragUnit>().OnEndDrag(eventData);
            Destroy(eventData.pointerDrag.GetComponent<DragUnit>());
        }
    }
}