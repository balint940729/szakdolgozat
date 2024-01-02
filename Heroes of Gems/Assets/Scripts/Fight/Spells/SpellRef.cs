// SpellScriptReference
using System;
using UnityEngine;

public class SpellRef {
    private SpellBaseClass spellInstance;

    //public void ActivateSpell() {
    //    if (spellInstance is SpellBaseClass) {
    //        spellInstance.InitializeSpell();
    //    }
    //    else {
    //        Debug.LogError("A spellLogic nem a SpellBaseClass-ből származik.");
    //    }
    //}

    public SpellBaseClass GetSpell() {
        return spellInstance;
    }

    public SpellRef(ScriptableObject spellSO) {
        SpellBaseSO spellBaseSO = spellSO as SpellBaseSO;

        if (spellBaseSO != null) {
            spellInstance = Activator.CreateInstance(spellBaseSO.spellLogic.GetClass(), spellBaseSO.spellName, spellBaseSO.spellDescription, spellBaseSO.spellImage) as SpellBaseClass;
        }
        else {
            Debug.LogError("A megadott objektum nem egy SpellBaseSO típusú ScriptableObject.");
        }
    }
}