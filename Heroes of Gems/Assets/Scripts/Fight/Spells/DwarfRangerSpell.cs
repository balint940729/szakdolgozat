using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DwarfRangerSpell : SpellBaseClass {

    public DwarfRangerSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        if (targetsGO.Count == 2) {
            foreach (GameObject targetGO in targetsGO) {
                UnitController target = targetGO.GetComponent<UnitController>();

                UnitController.NormalDamage(CalculateSpellDamage(target), target);
            }
        }
        else if (targetsGO.Count == 1) {
            UnitController target = targetsGO.First().GetComponent<UnitController>();
            for (int i = 0; i < 2; i++) {
                UnitController.NormalDamage(CalculateSpellDamage(target), target);
            }
        }
        else if (targetsGO.Count > 2) {
            //Get the first two element of the list
            for (int i = 0; i < 2; i++) {
                UnitController target = targetsGO.ElementAt(i).GetComponent<UnitController>();
                UnitController.NormalDamage(CalculateSpellDamage(target), target);
            }
        }
    }

    private bool IsUsingBlue(UnitController unit) {
        if (unit.GetColors().Find(color => color.colorName == "Blue") != null) {
            return true;
        }

        return false;
    }

    private bool IsBeast(UnitController unit) {
        if (unit.GetRace().raceName == "Beasts") {
            return true;
        }

        return false;
    }

    private int CalculateSpellDamage(UnitController target) {
        if (IsUsingBlue(target) && IsBeast(target)) {
            return caster.GetSpellDamage() * 4;
        }
        else if (IsUsingBlue(target) || IsBeast(target)) {
            return caster.GetSpellDamage() * 2;
        }

        return caster.GetSpellDamage();
    }
}