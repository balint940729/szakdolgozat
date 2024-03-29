﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WoodElfSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        UnitController weakestTarget = targetsGO[0].GetComponent<UnitController>();
        int weakestStat = weakestTarget.GetArmor() + weakestTarget.GetHealth();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();

            if (weakestStat > (target.GetArmor() + target.GetHealth())) {
                weakestStat = target.GetArmor() + target.GetHealth();
                weakestTarget = target;
            }
        }

        UnitController.NormalDamage(caster.GetSpellDamage(), weakestTarget);
    }
}