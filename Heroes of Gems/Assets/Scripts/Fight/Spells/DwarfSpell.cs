using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Dwarf")]
public class DwarfSpell : SpellBase {

    public DwarfSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        int iterator = 0;
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }
        IEnumerable<GameObject> targets = targetsGO;
        //Get the last two element of the list
        for (int i = targetsGO.Count - 1; i >= 0; i--) {
            if (iterator >= 2) {
                break;
            }
            UnitController target = targetsGO.ElementAt(i).GetComponent<UnitController>();
            caster.SpellAttack(caster.GetSpellDamage(), target);
            iterator++;
        }
    }
}