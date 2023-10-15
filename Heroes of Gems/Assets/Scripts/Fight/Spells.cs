using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class SpellBase : ScriptableObject {
    public string spellName;
    public string spellDescription;
    protected List<UnitController> enemyTargets;
    protected List<UnitController> allyTargets;
    protected UnitController caster;

    protected SpellBase(string spellName, string spellDescription) {
        this.spellName = spellName;
        this.spellDescription = spellDescription;
    }

    public virtual void setEnemyTargets(List<UnitController> units) {
        enemyTargets = units;
    }

    public virtual void setPlayerTargets(List<UnitController> units) {
        allyTargets = units;
    }

    public virtual void setCaster(UnitController caster) {
        this.caster = caster;
    }

    public abstract void InitializeSpell();

    public abstract void ChangeSpellDescription(int spellDamage);
}

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Wizard")]
public class WizardSpell : SpellBase {

    public WizardSpell(string spellName, string spellDescription) : base(spellName, spellDescription) {
    }

    public override void InitializeSpell() {
        foreach (UnitController target in enemyTargets) {
            caster.spellAttack(caster.GetSpellDamage(), target);
        }
    }

    public override void ChangeSpellDescription(int spellDamage) {
        spellDescription.Replace("&X", spellDamage.ToString());
    }
}

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Dog")]
public class DogSpell : SpellBase {

    public DogSpell(string spellName, string spellDescription) : base(spellName, spellDescription) {
    }

    public override void InitializeSpell() {
        UnitController target = enemyTargets.Last();
        caster.spellAttack(caster.GetSpellDamage(), target);
    }

    public override void ChangeSpellDescription(int spellDamage) {
        spellDescription.Replace("&X", spellDamage.ToString());
    }
}