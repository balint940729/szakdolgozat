using UnityEngine;
using UnityEngine.EventSystems;

public class TeamSlot : MonoBehaviour, IDropHandler {
    [SerializeField] private int index = 0;

    [SerializeField] private GameObject teamSlotGO = default;
    private RectTransform parent;

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag.name == "ItemButton") {
            //Unit draggedUnit = DragAndDrop.copyUnit.unit;
            UnitItem unitItem = GetComponent<UnitItem>();
            unitItem.unit = DragAndDrop.copyUnit.unit;

            Unit[] team = GetComponentInParent<Team>().team;
            GetComponent<TeamSlotDisplay>().SetMemberDisplay(unitItem.unit);
            team[index] = unitItem.unit;
            if (teamSlotGO.GetComponent<DragAndDrop>() == null) {
                teamSlotGO.AddComponent<DragAndDrop>();
                teamSlotGO.GetComponent<DragAndDrop>().originalGO = teamSlotGO;
                parent = (RectTransform)GameObject.Find("Inventory").transform;
                teamSlotGO.GetComponent<DragAndDrop>().SetParent(parent);
            }
        }

        if (eventData.pointerDrag.name.Contains("Teamslot")) {
            Unit tempUnit = null;
            UnitItem unitItem = GetComponent<UnitItem>();

            if (unitItem.unit == eventData.pointerDrag.GetComponent<UnitItem>().unit) {
                return;
            }

            if (unitItem.unit != null) {
                tempUnit = unitItem.unit;
            }

            unitItem.unit = DragAndDrop.copyUnit.unit;

            Unit[] team = GetComponentInParent<Team>().team;
            GetComponent<TeamSlotDisplay>().SetMemberDisplay(unitItem.unit);
            team[index] = unitItem.unit;

            //Switch places
            if (tempUnit != null) {
                eventData.pointerDrag.GetComponent<TeamSlotDisplay>().SetMemberDisplay(tempUnit);
                eventData.pointerDrag.GetComponent<UnitItem>().unit = tempUnit;
                team[eventData.pointerDrag.GetComponent<TeamSlot>().index] = unitItem.unit;
            }
            //Move to empty place
            else {
                eventData.pointerDrag.GetComponent<TeamSlotDisplay>().ResetTeamSlotDisplay();
                eventData.pointerDrag.GetComponent<UnitItem>().unit = null;
                team[eventData.pointerDrag.GetComponent<TeamSlot>().index] = null;
                eventData.pointerDrag.GetComponent<DragAndDrop>().OnEndDrag(eventData);
                Destroy(eventData.pointerDrag.GetComponent<DragAndDrop>());

                teamSlotGO.AddComponent<DragAndDrop>();
                teamSlotGO.GetComponent<DragAndDrop>().originalGO = teamSlotGO;
                parent = (RectTransform)GameObject.Find("Inventory").transform;
                teamSlotGO.GetComponent<DragAndDrop>().SetParent(parent);
            }
        }
    }
}