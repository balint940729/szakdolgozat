using UnityEngine;
using UnityEngine.EventSystems;

public class DropUnit : MonoBehaviour, IDropHandler {
    [SerializeField] private int index = 0;

    [SerializeField] private GameObject teamSlotGO = default;
    private RectTransform parent;

    public void OnDrop(PointerEventData eventData) {
        //From inventory to teamslot
        if (eventData.pointerDrag.name == "ItemButton") {
            UnitItem unitItem = GetComponent<UnitItem>();

            //Try add different unit
            if (unitItem.unit != DragUnit.copyUnit.unit) {
                Unit[] team = GetComponentInParent<Team>().team;
                if (team[index] != null) {
                    eventData.pointerDrag.GetComponentInParent<UnitsInventory>().AddUnit(unitItem.unit);
                    //eventData.pointerDrag.GetComponentInParent<UnitsInventory>().RemoveUnit(team[index]);
                }

                unitItem.unit = DragUnit.copyUnit.unit;

                GetComponent<TeamSlotDisplay>().SetMemberDisplay(unitItem.unit);
                team[index] = unitItem.unit;

                if (teamSlotGO.GetComponent<DragUnit>() == null) {
                    teamSlotGO.AddComponent<DragUnit>();
                    teamSlotGO.GetComponent<DragUnit>().originalGO = teamSlotGO;
                    parent = (RectTransform)GameObject.Find("Inventory").transform;
                    teamSlotGO.GetComponent<DragUnit>().SetParent(parent);
                }

                if (eventData.pointerDrag.GetComponentInParent<UnitsInventory>().UnitCount(unitItem.unit) == 1) {
                    eventData.pointerDrag.GetComponent<DragUnit>().OnEndDrag(eventData);
                    eventData.pointerDrag.GetComponentInParent<UnitsInventory>().RemoveUnit(unitItem.unit);
                }
                else
                    eventData.pointerDrag.GetComponentInParent<UnitsInventory>().RemoveUnit(unitItem.unit);
            }
        }

        //From teamslot to teamslot
        if (eventData.pointerDrag.name.Contains("Teamslot")) {
            Unit tempUnit = null;
            UnitItem unitItem = GetComponent<UnitItem>();

            if (unitItem.unit == eventData.pointerDrag.GetComponent<UnitItem>().unit) {
                return;
            }

            if (unitItem.unit != null) {
                tempUnit = unitItem.unit;
            }

            unitItem.unit = DragUnit.copyUnit.unit;

            Team teamGO = GetComponentInParent<Team>();
            Team fromTeamGO = eventData.pointerDrag.GetComponentInParent<Team>();
            GetComponent<TeamSlotDisplay>().SetMemberDisplay(unitItem.unit);
            teamGO.team[index] = unitItem.unit;

            //Switch places
            if (tempUnit != null) {
                eventData.pointerDrag.GetComponent<TeamSlotDisplay>().SetMemberDisplay(tempUnit);
                eventData.pointerDrag.GetComponent<UnitItem>().unit = tempUnit;
                fromTeamGO.team[eventData.pointerDrag.GetComponent<DropUnit>().index] = tempUnit;
                teamGO.team[index] = unitItem.unit;
            }
            //Move to empty place
            else {
                eventData.pointerDrag.GetComponent<TeamSlotDisplay>().ResetTeamSlotDisplay();
                eventData.pointerDrag.GetComponent<UnitItem>().unit = null;
                fromTeamGO.team[eventData.pointerDrag.GetComponent<DropUnit>().index] = null;
                eventData.pointerDrag.GetComponent<DragUnit>().OnEndDrag(eventData);
                Destroy(eventData.pointerDrag.GetComponent<DragUnit>());

                teamSlotGO.AddComponent<DragUnit>();
                teamSlotGO.GetComponent<DragUnit>().originalGO = teamSlotGO;
                parent = (RectTransform)GameObject.Find("Inventory").transform;
                teamSlotGO.GetComponent<DragUnit>().SetParent(parent);
            }
        }
    }
}