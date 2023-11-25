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

    //private bool isTextChanged = false;

    // Start is called before the first frame update
    private void Start() {
        //health.text = card.baseHealth.ToString();
        //attack.text = card.baseAttack.ToString();
        //armor.text = card.baseArmor.ToString();
        //mana.text = card.currentMana.ToString() + "/" + card.maxMana.ToString();

        image.sprite = card.image;
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

    public void SetStats(int newHealth, int newArmor, int newAttack, int newMana) {
        health.text = newHealth.ToString();
        attack.text = newAttack.ToString();
        armor.text = newArmor.ToString();
        mana.text = newMana.ToString() + "/" + card.maxMana.ToString();
    }

    public void SetStats(int newHealth, int newArmor, int newAttack) {
        health.text = newHealth.ToString();
        armor.text = newArmor.ToString();
        attack.text = newAttack.ToString();
    }

    public void SetStats(int newHealth, int newArmor) {
        health.text = newHealth.ToString();
        armor.text = newArmor.ToString();
    }

    public void SetHealth(int newHealth) {
        health.text = newHealth.ToString();
    }

    public void SetArmor(int newArmor) {
        armor.text = newArmor.ToString();
    }

    public void SetAttack(int newAttack) {
        attack.text = newAttack.ToString();
        //isTextChanged = true;
    }

    public void SetMana(int newCurrentMana) {
        mana.text = newCurrentMana.ToString() + "/" + card.maxMana.ToString();
    }
}