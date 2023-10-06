using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
[System.Serializable]
public class Unit : ScriptableObject {
    public string baseName;

    public int baseHealth;

    public int baseAttack;

    public int baseArmor;

    public int maxMana;
    public int currentMana;

    public int baseSpellDamage;

    public Sprite image;

    public List<Colors> colors;
}