﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GiantSpiderSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifyAttack(-caster.GetSpellDamage());
        }

        UnitController lastEnemy = targetsGO.Last().GetComponent<UnitController>();
        UnitController.NormalDamage(caster.GetSpellDamage(), lastEnemy);
    }
}