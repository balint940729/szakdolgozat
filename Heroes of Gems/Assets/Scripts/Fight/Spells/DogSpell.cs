using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DogSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();
        UnitController.NormalDamage(caster.GetSpellDamage(), target);
    }
}