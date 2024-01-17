using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag.name.Contains("Teamslot")) {
            Unit[] team = eventData.pointerDrag.GetComponentInParent<Team>().team;
            eventData.pointerDrag.GetComponent<TeamSlotDisplay>().ResetTeamSlotDisplay();
            eventData.pointerDrag.GetComponent<UnitItem>().unit = null;

            int.TryParse(eventData.pointerDrag.name.Substring(eventData.pointerDrag.name.Length), out int index);
            team[index] = null;
            eventData.pointerDrag.GetComponent<DragAndDrop>().OnEndDrag(eventData);
            Destroy(eventData.pointerDrag.GetComponent<DragAndDrop>());
        }
    }
}