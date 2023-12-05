using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Treefolk")]
public class TreefolkSpell : SpellBase {

    public TreefolkSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }
        UnitController target = targetsGO.Last().GetComponent<UnitController>();
        caster.ModifyHealth(caster.GetSpellDamage());
        caster.NormalDamage(caster.GetSpellDamage(), target);
    }
}