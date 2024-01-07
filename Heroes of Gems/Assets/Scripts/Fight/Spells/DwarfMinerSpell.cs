using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DwarfMinerSpell : SpellBaseClass {

    public DwarfMinerSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        int iterator = 0;
        List<GameObject> targetsGO = GetOppenentTeam();

        if (targetsGO.Count == 2) {
            foreach (GameObject targetGO in targetsGO) {
                UnitController target = targetGO.GetComponent<UnitController>();
                UnitController.NormalDamage(caster.GetSpellDamage(), target);
            }
        }
        else if (targetsGO.Count == 1) {
            UnitController target = targetsGO.First().GetComponent<UnitController>();
            for (int i = 0; i < 2; i++) {
                UnitController.NormalDamage(caster.GetSpellDamage(), target);
            }
        }
        else if (targetsGO.Count > 2) {
            //Get the last two element of the list
            for (int i = targetsGO.Count - 1; i >= 0; i--) {
                if (iterator >= 2) {
                    break;
                }
                UnitController target = targetsGO.ElementAt(i).GetComponent<UnitController>();
                UnitController.NormalDamage(caster.GetSpellDamage(), target);
                iterator++;
            }
        }
    }
}