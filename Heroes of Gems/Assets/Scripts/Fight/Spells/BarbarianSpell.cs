using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Barbarian")]
public class BarbarianSpell : SpellBase {

    public BarbarianSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        caster.ModifyAttack(caster.GetSpellDamage());

        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }
        UnitController target = targetsGO.First().GetComponent<UnitController>();
        caster.NormalDamage(caster.GetAttack(), target);
    }
}