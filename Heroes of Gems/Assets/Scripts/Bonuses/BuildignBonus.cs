using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildignBonus : BaseBonus {

    public static void InitializeBonus(List<GameObject> team) {
        List<BuildingController> activeBuildings = BuildingController.GetActiveBuildings();

        foreach (BuildingController building in activeBuildings) {
            foreach (GameObject memberGO in team) {
                UnitController unit = memberGO.GetComponent<UnitController>();
                if (building.GetAffectedRaces().Count == 0) {
                    ModifyStat(unit, building);
                }
                else {
                    foreach (Race race in building.GetAffectedRaces()) {
                        if (unit.GetRace() == race) {
                            ModifyStat(unit, building);
                        }
                    }
                }
            }
        }
    }

    private static void ModifyStat(UnitController unit, BuildingController building) {
        int modifier = building.GetTotalBonusModify();

        foreach (ModifStats stat in building.GetBonusStats()) {
            switch (stat) {
                case ModifStats.Attack:
                    unit.ModifyAttack(modifier);
                    break;

                case ModifStats.Armor:
                    unit.ModifyArmor(modifier);
                    break;

                case ModifStats.Health:
                    unit.ModifyHealth(modifier);
                    break;

                case ModifStats.SpellDamage:
                    unit.ModifySpellDamage(modifier);
                    break;

                case ModifStats.Mana:
                    unit.GainMana(modifier);
                    break;

                default:
                    break;
            }
        }
    }
}