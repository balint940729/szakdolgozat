using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Shade")]
public class ShadeSpell : SpellBase {

    public ShadeSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }

        UnitController target = targetsGO.First().GetComponent<UnitController>();

        int drainedLifeAmount = target.GetHealth() - caster.GetSpellDamage();

        if (drainedLifeAmount > 1) {
            caster.ModifyHealth(target.GetHealth());
        }
        else {
            caster.ModifyHealth(caster.GetSpellDamage());
        }

        caster.TrueDamage(caster.GetSpellDamage(), target);
    }
}