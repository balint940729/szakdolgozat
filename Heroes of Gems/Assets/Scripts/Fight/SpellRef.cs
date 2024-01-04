using System;
using UnityEngine;

public class SpellRef {
    private SpellBaseClass spellInstance;

    public SpellBaseClass GetSpell() {
        return spellInstance;
    }

    public SpellRef(ScriptableObject spellSO) {
        SpellBaseSO spellBaseSO = spellSO as SpellBaseSO;

        if (spellBaseSO != null) {
            spellInstance = Activator.CreateInstance(spellBaseSO.spellLogic.GetClass(), spellBaseSO.spellName, spellBaseSO.spellDescription, spellBaseSO.spellImage) as SpellBaseClass;
        }
        else {
            Debug.LogError("The object is not a SpellBaseSO Scriptable Object.");
        }
    }
}