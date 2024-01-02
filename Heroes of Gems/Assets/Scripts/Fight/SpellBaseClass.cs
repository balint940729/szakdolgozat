using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SpellBaseClass : ScriptableObject {
    //public SpellBaseSO spellSO;

    protected string spellName;
    protected string spellDescription;
    protected Sprite spellImage;

    protected List<UnitController> targets;
    protected UnitController caster;

    public SpellBaseClass() {
    }

    protected SpellBaseClass(string spellName, string spellDescription, Sprite spellImage) {
        this.spellName = spellName;
        this.spellDescription = spellDescription;
        this.spellImage = spellImage;
    }

    public virtual void SetTargets(List<UnitController> units) {
        //public virtual void setEnemyTargets(List<UnitController> units) {
        targets = units;
        //enemyTargets = units;
    }

    //public virtual void setPlayerTargets(List<UnitController> units) {
    //    allyTargets = units;
    //}

    public virtual void SetCaster(UnitController caster) {
        this.caster = caster;
    }

    public virtual void InitializeSpell() {
    }

    public virtual bool IsSpellTargets() {
        return false;
    }
}