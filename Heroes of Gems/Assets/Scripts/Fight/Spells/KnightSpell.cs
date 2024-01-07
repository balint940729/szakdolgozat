﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KnightSpell : SpellBaseClass {

    public KnightSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();

        if (target.GetRace().raceName != "Humans") {
            UnitController.NormalDamage(caster.GetSpellDamage() * 2, target);
            return;
        }

        UnitController.NormalDamage(caster.GetSpellDamage(), target);
    }
}