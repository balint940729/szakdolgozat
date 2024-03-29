﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SorcererSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();
        List<GameObject> alliesGO = GetOpponentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();
        UnitController firstAlly = alliesGO.First().GetComponent<UnitController>();

        firstAlly.ModifySpellDamage(caster.GetSpellDamage());

        UnitController.NormalDamage(firstAlly.GetSpellDamage(), target);
    }
}