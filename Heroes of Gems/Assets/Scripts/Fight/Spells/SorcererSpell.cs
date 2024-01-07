using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SorcererSpell : SpellBaseClass {

    public SorcererSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();
        List<GameObject> alliesGO = GetOppenentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();
        UnitController firstAlly = alliesGO.First().GetComponent<UnitController>();

        firstAlly.ModifySpellDamage(caster.GetSpellDamage());

        UnitController.NormalDamage(firstAlly.GetSpellDamage(), target);
    }
}