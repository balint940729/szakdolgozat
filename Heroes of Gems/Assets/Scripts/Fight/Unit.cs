using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Unit : ScriptableObject
{

    public string unitName;

    public int unitHealth;

    public int unitAttack;

    public int unitArmor;

    public int unitMaxMana;
    public int unitCurrentMana;

    public int unitSpellDamage;

    public Sprite unitImage;


    //public Unit(string name, int health, int attack, int armor, int maxMana, int currentMana, int spellDamage, Sprite image)
    //{
    //    unitName = name;
    //    unitImage = image;
    //    // Unit health is 0 or greater
    //    if (health < 0)
    //    {
    //        unitHealth = 0;
    //    }
    //    else
    //    {
    //        unitHealth = health;
    //    }

    //    // Unit Attack is 0 or greater
    //    if (attack < 0)
    //    {
    //        unitAttack = 0;
    //    }
    //    else
    //    {
    //        unitAttack = attack;
    //    }

    //    // Unit Armor is 0 or greater
    //    if (armor < 0)
    //    {
    //        unitArmor = 0;
    //    }
    //    else
    //    {
    //        unitArmor = armor;
    //    }

    //    // Unit Maximum mana is 0 or greater
    //    if (maxMana < 0)
    //    {
    //        unitMaxMana = 0;
    //    }
    //    else
    //    {
    //        unitMaxMana = maxMana;
    //    }

    //    // Unit Current mana is 0 or lower than Maximum mana
    //    if (currentMana < 0)
    //    {
    //        unitCurrentMana = 0;
    //    }
    //    else if (currentMana > maxMana)
    //    {
    //        unitCurrentMana = maxMana;
    //    }
    //    else
    //    {
    //        unitCurrentMana = currentMana;
    //    }

    //    // Unit Maximum mana is 0 or greater
    //    if (spellDamage < 0)
    //    {
    //        unitSpellDamage = 0;
    //    }
    //    else
    //    {
    //        unitSpellDamage = spellDamage;
    //    }
    //}
}
