using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDisplay : MonoBehaviour {
    public Unit card;

    public TMP_Text health;
    public TMP_Text attack;
    public TMP_Text armor;

    public Image image;

    // Start is called before the first frame update
    private void Start() {
        health.text = card.unitHealth.ToString();
        attack.text = card.unitAttack.ToString();
        armor.text = card.unitArmor.ToString();

        image.sprite = card.unitImage;
    }

    public void showDamage(int newHealth, int newArmor) {
        health.text = newHealth.ToString();
        armor.text = newArmor.ToString();
    }
}