using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Wizard")]
public class WizardSpell : SpellBase {

    public WizardSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        foreach (UnitController target in enemyTargets) {
            caster.spellAttack(caster.GetSpellDamage(), target);
        }
    }
}