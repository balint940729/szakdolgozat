using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighElfSpell : SpellBaseClass {
    //public string spellName;
    //public string spellDescription;
    //public Sprite spellImage;
    //protected List<UnitController> targets;
    //protected UnitController caster;

    //public HighElfSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    //}

    public HighElfSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifyHealth(caster.GetSpellDamage());
        }
    }
}