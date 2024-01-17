using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamSlotDisplay : MonoBehaviour {
    [SerializeField] private Image memberImage = default;
    [SerializeField] private TMP_Text memberName = default;

    private void Awake() {
        memberImage.color = Color.gray;
        memberName.text = "Empty";
    }

    public void SetMemberDisplay(Unit unit) {
        memberImage.sprite = unit.image;
        memberImage.color = Color.white;

        memberName.text = unit.baseName;
    }

    public void ResetTeamSlotDisplay() {
        memberImage.sprite = null;
        memberImage.color = Color.gray;
        memberName.text = "Empty";
    }
}