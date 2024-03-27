using TMPro;
using UnityEngine;

public class GoldDisplay : MonoBehaviour {
    [SerializeField] private TMP_Text goldCounter = default;

    public void Start() {
        goldCounter.text = GoldController.GetGold().ToString();
    }

    public void Update() {
        int.TryParse(goldCounter.text, out int count);

        if (GoldController.GetGold() != count) {
            ChangeGoldCounter();
        }
    }

    public void ChangeGoldCounter() {
        goldCounter.text = GoldController.GetGold().ToString();
    }
}