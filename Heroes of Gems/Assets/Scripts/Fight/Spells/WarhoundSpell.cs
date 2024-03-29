﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarhoundSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifyAttack(-caster.GetSpellDamage());

            if (target.GetRace().raceName != "Undeads") {
                UnitController.NormalDamage(caster.GetSpellDamage(), target);
            }
        }
    }
}