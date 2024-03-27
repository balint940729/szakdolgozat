using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceBonus : BaseBonus {

    public static void InitializeBonus(List<GameObject> team) {
        Dictionary<Race, int> raceNumber = new Dictionary<Race, int>();

        foreach (GameObject memberGO in team) {
            UnitController member = memberGO.GetComponent<UnitController>();
            Race race = member.GetRace();

            if (raceNumber.ContainsKey(race)) {
                raceNumber[race]++;
            }
            else {
                raceNumber[race] = 1;
            }
        }

        foreach (GameObject memberGO in team) {
            UnitController unit = memberGO.GetComponent<UnitController>();
            ModifyStat(unit, raceNumber);
        }
    }

    protected static void ModifyStat(UnitController unit, Dictionary<Race, int> raceNumber) {
        Race race = unit.GetRace();
        int modifier = unit.GetRace().raceBonus.bonusModifier;

        foreach (ModifStats stat in unit.GetRace().raceBonus.bonusStats) {
            switch (stat) {
                case ModifStats.Attack:
                    unit.ModifyAttack(modifier * raceNumber[race]);
                    break;

                case ModifStats.Armor:
                    unit.ModifyArmor(modifier * raceNumber[race]);
                    break;

                case ModifStats.Health:
                    unit.ModifyHealth(modifier * raceNumber[race]);
                    break;

                case ModifStats.SpellDamage:
                    unit.ModifySpellDamage(modifier * raceNumber[race]);
                    break;

                case ModifStats.Mana:
                    unit.GainMana(modifier * raceNumber[race]);
                    break;

                default:
                    break;
            }
        }
    }

    public override void SetBonusDescription() {
        base.SetBonusDescription();
        bonusDescription += " The bonus is stronger when more members share the same Race.";
    }
}