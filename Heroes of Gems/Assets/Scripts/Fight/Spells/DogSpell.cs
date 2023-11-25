using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Dog")]
public class DogSpell : SpellBase {

    public DogSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().getEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().getPlayerTeam();
        }
        UnitController target = targetsGO.First().GetComponent<UnitController>();
        caster.spellAttack(caster.GetSpellDamage(), target);
    }

    //public override bool isSpellTargets() {
    //    return true;
    //}
}