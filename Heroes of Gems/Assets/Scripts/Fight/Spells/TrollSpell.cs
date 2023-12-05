using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Troll")]
public class TrollSpell : SpellBase {

    public TrollSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifyArmor(-caster.GetSpellDamage());
        }
    }
}