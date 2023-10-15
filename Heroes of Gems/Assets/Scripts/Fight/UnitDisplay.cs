using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDisplay : MonoBehaviour {
    public Unit card;

    public TMP_Text health;
    public TMP_Text attack;
    public TMP_Text armor;
    public TMP_Text mana;

    public Image image;

    // Start is called before the first frame update
    private void Start() {
        health.text = card.baseHealth.ToString();
        attack.text = card.baseAttack.ToString();
        armor.text = card.baseArmor.ToString();
        mana.text = card.currentMana.ToString() + "/" + card.maxMana.ToString();

        image.sprite = card.image;
    }

    public void setHealth(int newHealth, int newArmor) {
        health.text = newHealth.ToString();
        armor.text = newArmor.ToString();
    }

    public void setMana(int newCurrentMana) {
        mana.text = newCurrentMana.ToString() + "/" + card.maxMana.ToString();
    }

    public void setColors(GameObject manaGO, int index) {
        Image manaImage = manaGO.GetComponent<Image>();

        manaImage.type = Image.Type.Filled;
        manaImage.fillMethod = Image.FillMethod.Radial360;
        manaImage.fillAmount = (1.0f / card.colors.Count) * (index + 1);
        manaImage.fillClockwise = true;
        manaImage.fillOrigin = (int)Image.Origin360.Top;
        manaImage.color = card.colors[index].color;
    }
}