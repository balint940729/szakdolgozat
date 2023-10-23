using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class SpellBase : ScriptableObject {
    public string spellName;
    public string spellDescription;
    public Sprite spellImage;
    protected List<UnitController> enemyTargets;
    protected List<UnitController> allyTargets;
    protected UnitController caster;

    protected SpellBase(string spellName, string spellDescription, Sprite spellImage) {
        this.spellName = spellName;
        this.spellDescription = spellDescription;
        this.spellImage = spellImage;
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

    public virtual void ChangeSpellDescription(int spellDamage) {
        spellDescription = spellDescription.Replace("&X", spellDamage.ToString());
    }
}