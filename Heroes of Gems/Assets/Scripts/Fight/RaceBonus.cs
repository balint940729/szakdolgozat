using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceBonus : BaseBonus {
    private Dictionary<Race, int> raceNumber = new Dictionary<Race, int>();

    public override void InitializeBonus(List<GameObject> team) {
        CountRaceMembers(team);
        foreach (GameObject memberGO in team) {
            UnitController unit = memberGO.GetComponent<UnitController>();
            ModifyStat(unit);
        }
    }

    protected override void BonusDescription() {
        base.BonusDescription();
        bonusDescription += " The bonus is stronger when more members share the same Race.";
    }

    protected override void ModifyStat(UnitController unit) {
        Race race = unit.GetRace();

        foreach (ModifStats stat in bonusStats) {
            switch (stat) {
                case ModifStats.Attack:
                    unit.ModifyAttack(bonusModifier * raceNumber[race]);
                    break;

                case ModifStats.Armor:
                    unit.ModifyArmor(bonusModifier * raceNumber[race]);
                    break;

                case ModifStats.Health:
                    unit.ModifyHealth(bonusModifier * raceNumber[race]);
                    break;

                case ModifStats.SpellDamage:
                    unit.ModifySpellDamage(bonusModifier * raceNumber[race]);
                    break;

                case ModifStats.Mana:
                    unit.GainMana(bonusModifier * raceNumber[race]);
                    break;

                default:
                    break;
            }
        }
    }

    private void CountRaceMembers(List<GameObject> team) {
        foreach (GameObject memberGO in team) {
            UnitController member = memberGO.GetComponent<UnitController>();

            AddRaceMember(member.GetRace());

            //if (member.GetRace() == race) {
            //    count++;
            //}
        }
    }

    private void AddRaceMember(Race race) {
        if (raceNumber.ContainsKey(race)) {
            raceNumber[race]++;
        }
        else {
            raceNumber[race] = 1;
        }
    }

    //[HideInInspector]
    //public string raceBonusDescription;

    //public List<ModifStats> raceBonusStat;
    //public int raceBonusModifier;
    //public Race race;

    //public RaceBonus(List<ModifStats> raceBonusStat, int raceBonusModifier, Race race) {
    //    this.raceBonusStat = raceBonusStat;
    //    this.raceBonusModifier = raceBonusModifier;
    //    this.race = race;
    //    raceBonusDescription = SetRaceBonusDescription();
    //}

    //public virtual void InitializeRaceBonus(List<GameObject> team, UnitController unit) {
    //    int unitsNumber = CountRaceMembers(team);

    //    foreach (ModifStats raceStat in raceBonusStat) {
    //        switch (raceStat) {
    //            case ModifStats.Attack:
    //                unit.ModifyAttack(raceBonusModifier * unitsNumber);
    //                break;

    //            case ModifStats.Armor:
    //                unit.ModifyArmor(raceBonusModifier * unitsNumber);
    //                break;

    //            case ModifStats.Health:
    //                unit.ModifyHealth(raceBonusModifier * unitsNumber);
    //                break;

    //            case ModifStats.SpellDamage:
    //                unit.ModifySpellDamage(raceBonusModifier * unitsNumber);
    //                break;

    //            case ModifStats.Mana:
    //                unit.GainMana(raceBonusModifier * unitsNumber);
    //                break;

    //            default:
    //                break;
    //        }
    //    }
    //}

    //private string SetRaceBonusDescription() {
    //    raceBonusDescription = "All " + race + " gain: ";
    //    foreach (ModifStats stats in raceBonusStat) {
    //        raceBonusDescription += "+" + raceBonusModifier + " " + stats;

    //        if (stats == raceBonusStat.Last()) {
    //            raceBonusDescription += ".";
    //            return raceBonusDescription;
    //        }

    //        raceBonusDescription += ", ";
    //    }

    //    return raceBonusDescription;
    //}
}