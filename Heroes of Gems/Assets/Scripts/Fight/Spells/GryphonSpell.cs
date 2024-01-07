using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GryphonSpell : SpellBaseClass {

    public GryphonSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();
        List<GameObject> alliesGO = GetOppenentTeam();

        int blueYellowAllies = CountBlueAndYellowUnits(alliesGO);

        if (targetsGO.Count <= 3) {
            foreach (GameObject targetGO in targetsGO) {
                UnitController target = targetGO.GetComponent<UnitController>();

                UnitController.NormalDamage(caster.GetSpellDamage() * (blueYellowAllies > 0 ? blueYellowAllies - 1 : 1), target);
            }
        }
        else {
            //Get the first 3 element of the list
            for (int i = 0; i < 3; i++) {
                UnitController target = targetsGO.ElementAt(i).GetComponent<UnitController>();
                UnitController.NormalDamage(caster.GetSpellDamage() * (blueYellowAllies > 0 ? blueYellowAllies - 1 : 1), target);
            }
        }
    }

    private int CountBlueAndYellowUnits(List<GameObject> team) {
        int count = 0;
        foreach (GameObject unitGO in team) {
            UnitController unit = unitGO.GetComponent<UnitController>();

            if (IsUsingBlueOrYellow(unit)) {
                count++;
            }
        }

        return count;
    }

    private bool IsUsingBlueOrYellow(UnitController unit) {
        if (unit.GetColors().Find(color => color.colorName == "Blue" || color.colorName == "Yellow") != null) {
            return true;
        }

        return false;
    }
}