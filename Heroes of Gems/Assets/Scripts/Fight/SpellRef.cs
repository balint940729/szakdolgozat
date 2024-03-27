using UnityEngine;

public class SpellRef {
    private SpellBaseClass spellInstance;

    public SpellBaseClass GetSpell() {
        return spellInstance;
    }

    public SpellRef(ScriptableObject spellSO) {
        SpellBaseSO spellBaseSO = spellSO as SpellBaseSO;

        if (spellBaseSO != null) {
            spellInstance = ScriptableObject.CreateInstance(spellBaseSO.spellLogic.GetClass()) as SpellBaseClass;
        }
        else {
            Debug.LogError("The object is not a SpellBaseSO Scriptable Object.");
        }
    }
}