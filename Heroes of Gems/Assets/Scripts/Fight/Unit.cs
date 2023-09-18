using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
[System.Serializable]
public class Unit : ScriptableObject {
    public string unitName;

    public int unitHealth;

    public int unitAttack;

    public int unitArmor;

    public int unitMaxMana;
    public int unitCurrentMana;

    public int unitSpellDamage;

    public Sprite unitImage;
}