using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
[System.Serializable]
public class Unit : Lootable {
    public string baseName;

    public int baseHealth;

    public int baseAttack;

    public int baseArmor;

    public int maxMana;
    public int currentMana;

    public int baseSpellDamage;

    public Sprite image;

    public List<Colors> colors;

    public SpellBaseSO spell;

    public Race race;
}