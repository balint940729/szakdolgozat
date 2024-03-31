using UnityEngine;

[System.Serializable]
public class Team : MonoBehaviour {
    [SerializeField] private Unit[] team = default;

    [HideInInspector] [SerializeField] private bool isSelected = false;

    public Unit[] GetTeam() {
        return team;
    }

    public void SetTeam(Unit[] team) {
        this.team = team;
    }

    public void SetMember(Unit unit, int index) {
        team[index] = unit;
    }

    public void SetSelected(bool isSelected) {
        this.isSelected = isSelected;
    }

    public Unit GetMember(int index) {
        return team[index];
    }

    public bool IsSelected() {
        return isSelected;
    }

    //[SerializeField] public TeamObjectData team = default;

    //private void Awake() {
    //    team = new TeamObjectData(members);
    //}
}