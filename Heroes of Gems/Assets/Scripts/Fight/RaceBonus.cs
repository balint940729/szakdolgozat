using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RaceStats { Attack, Armor, Health, SpellDamage, Mana };

[System.Serializable]
[CreateAssetMenu(fileName = "RaceBonus", menuName = "RaceBonus")]
public class RaceBonus : ScriptableObject {

    [HideInInspector]
    public string raceBonusDescription;

    public List<RaceStats> raceBonusStat;
    public int raceBonusModifier;
    public Race race;

    public RaceBonus(List<RaceStats> raceBonusStat, int raceBonusModifier, Race race) {
        this.raceBonusStat = raceBonusStat;
        this.raceBonusModifier = raceBonusModifier;
        this.race = race;
        raceBonusDescription = SetRaceBonusDescription();
    }

    public virtual void InitializeRaceBonus(List<GameObject> team, UnitController unit) {
        int unitsNumber = CountRaceMembers(team);

        foreach (RaceStats raceStat in raceBonusStat) {
            switch (raceStat) {
                case RaceStats.Attack:
                    unit.ModifyAttack(raceBonusModifier * unitsNumber);
                    break;

                case RaceStats.Armor:
                    unit.ModifyArmor(raceBonusModifier * unitsNumber);
                    break;

                case RaceStats.Health:
                    unit.ModifyHealth(raceBonusModifier * unitsNumber);
                    break;

                case RaceStats.SpellDamage:
                    unit.ModifySpellDamage(raceBonusModifier * unitsNumber);
                    break;

                case RaceStats.Mana:
                    unit.GainMana(raceBonusModifier * unitsNumber);
                    break;

                default:
                    break;
            }
        }
    }

    private int CountRaceMembers(List<GameObject> team) {
        int count = 0;

        foreach (GameObject memberGO in team) {
            UnitController member = memberGO.GetComponent<UnitController>();

            if (member.GetRace() == race) {
                count++;
            }
        }

        return count;
    }

    private string SetRaceBonusDescription() {
        raceBonusDescription = "All " + race + " gain: ";
        foreach (RaceStats stats in raceBonusStat) {
            raceBonusDescription += "+" + raceBonusModifier + " " + stats;

            if (stats == raceBonusStat.Last()) {
                raceBonusDescription += ".";
                return raceBonusDescription;
            }

            raceBonusDescription += ", ";
        }

        return raceBonusDescription;
    }
}