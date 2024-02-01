using TMPro;
using UnityEngine;

public class GoldDisplay : MonoBehaviour {
    [SerializeField] private TMP_Text goldCounter = default;

    public void Start() {
        goldCounter.text = GoldController.GetGold().ToString();
    }

    public void ChangeGoldCounter() {
        goldCounter.text = GoldController.GetGold().ToString();
    }
}