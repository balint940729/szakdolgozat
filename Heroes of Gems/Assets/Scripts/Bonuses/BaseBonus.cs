using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ModifStats { Attack, Armor, Health, SpellDamage, Mana };

[System.Serializable]
public class BaseBonus {

    [HideInInspector]
    public string bonusDescription;

    public List<Race> affectedRaces;
    public List<ModifStats> bonusStats;
    public int bonusModifier;

    public virtual string GetBonusDescription() {
        SetBonusDescription();
        return bonusDescription;
    }

    public virtual void SetBonusDescription() {
        string races = "";
        if (affectedRaces.Count == 0) {
            races += "Everyone";
        }
        else if (affectedRaces.Count == 1) {
            races += "All " + affectedRaces[0].raceName;
        }
        else if (affectedRaces.Count > 1) {
            races += "All " + string.Join(", ", affectedRaces.Take(affectedRaces.Count - 1)) + " and " + affectedRaces.Last();
        }

        string stats = "";
        if (bonusStats.Count == 1) {
            stats += bonusStats[0].ToString();
        }
        else if (bonusStats.Count > 1) {
            stats += string.Join(", ", bonusStats.Take(bonusStats.Count - 1)) + " and " + bonusStats.Last();
        }

        bonusDescription = $"{races} in your team gain +{bonusModifier} {stats}"; ;
    }

    public virtual void SetBonusDescription(int bonusmodifier) {
        string races = "";
        if (affectedRaces.Count == 0) {
            races += "Everyone";
        }
        else if (affectedRaces.Count == 1) {
            races += "All " + affectedRaces[0].raceName;
        }
        else if (affectedRaces.Count > 1) {
            races += "All " + string.Join(", ", affectedRaces.Take(affectedRaces.Count - 1)) + " and " + affectedRaces.Last();
        }

        string stats = "";
        if (bonusStats.Count == 1) {
            stats += bonusStats[0].ToString();
        }
        else if (bonusStats.Count > 1) {
            stats += string.Join(", ", bonusStats.Take(bonusStats.Count - 1)) + " and " + bonusStats.Last();
        }

        bonusDescription = $"{races} in your team gain +{bonusModifier} {stats}"; ;
    }
}