﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SalamanderSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            if (target.GetColors().Find(color => color.colorName == "Green") != null) {
                UnitController.NormalDamage(caster.GetSpellDamage() * 2, target);
                continue;
            }
            UnitController.NormalDamage(caster.GetSpellDamage(), target);
        }
    }
}