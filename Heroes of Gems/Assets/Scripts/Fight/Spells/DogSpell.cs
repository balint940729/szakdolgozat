using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Dog")]
public class DogSpell : SpellBase {

    public DogSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        UnitController target = enemyTargets.Last();
        caster.spellAttack(caster.GetSpellDamage(), target);
    }
}