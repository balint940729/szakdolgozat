using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SpellBaseClass : ScriptableObject {
    protected UnitController caster;

    public virtual void SetCaster(UnitController caster) {
        this.caster = caster;
    }

    public abstract void InitializeSpell();

    public virtual bool IsSpellTargets() {
        return false;
    }

    public virtual List<GameObject> GetOpponentTeam() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }

        return targetsGO;
    }

    public virtual List<GameObject> GetAllyTeam() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }

        return targetsGO;
    }
}