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

    public virtual void InitializeBonus(List<GameObject> team) {
        if (affectedRaces.Count == 0) {
            foreach (GameObject memberGO in team) {
                UnitController unit = memberGO.GetComponent<UnitController>();
                ModifyStat(unit);
            }

            return;
        }

        foreach (Race race in affectedRaces) {
            foreach (GameObject memberGO in team) {
                UnitController unit = memberGO.GetComponent<UnitController>();
                if (unit.GetRace() == race) {
                    ModifyStat(unit);
                }
            }
        }
    }

    public static void InitializeBonus2(List<GameObject> team) {
        foreach (GameObject memberGO in team) {
            UnitController unit = memberGO.GetComponent<UnitController>();
            ModifyStat2(unit);
        }
    }

    protected static void ModifyStat2(UnitController unit) {
        List<ModifStats> stats = unit.GetRace().raceBonus.bonusStats;
        int bonusModif = unit.GetRace().raceBonus.bonusModifier;

        foreach (ModifStats stat in stats) {
            switch (stat) {
                case ModifStats.Attack:
                    unit.ModifyAttack(bonusModif);
                    break;

                case ModifStats.Armor:
                    unit.ModifyArmor(bonusModif);
                    break;

                case ModifStats.Health:
                    unit.ModifyHealth(bonusModif);
                    break;

                case ModifStats.SpellDamage:
                    unit.ModifySpellDamage(bonusModif);
                    break;

                case ModifStats.Mana:
                    unit.GainMana(bonusModif);
                    break;

                default:
                    break;
            }
        }
    }

    protected virtual void ModifyStat(UnitController unit) {
        foreach (ModifStats stat in bonusStats) {
            switch (stat) {
                case ModifStats.Attack:
                    unit.ModifyAttack(bonusModifier);
                    break;

                case ModifStats.Armor:
                    unit.ModifyArmor(bonusModifier);
                    break;

                case ModifStats.Health:
                    unit.ModifyHealth(bonusModifier);
                    break;

                case ModifStats.SpellDamage:
                    unit.ModifySpellDamage(bonusModifier);
                    break;

                case ModifStats.Mana:
                    unit.GainMana(bonusModifier);
                    break;

                default:
                    break;
            }
        }
    }

    protected virtual void BonusDescription() {
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