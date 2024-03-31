using System.Collections.Generic;
using UnityEngine;

public class EquipmentBonus : BaseBonus {

    public static void InitializeBonus(List<GameObject> team) {
        foreach (GameObject memberGO in team) {
            UnitController unit = memberGO.GetComponent<UnitController>();
            foreach (EquipmentsObjectData equipment in Equipments.GetEquipments()) {
                if (equipment.item.bonus.affectedRaces.Count == 0) {
                    ModifyStat(unit, equipment.item.bonus);
                }
                else {
                    foreach (Race race in equipment.item.bonus.affectedRaces) {
                        if (unit.GetRace() == race) {
                            ModifyStat(unit, equipment.item.bonus);
                        }
                    }
                }
            }
        }
    }

    private static void ModifyStat(UnitController unit, BaseBonus bonus) {
        int modifier = bonus.bonusModifier;

        foreach (ModifStats stat in bonus.bonusStats) {
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