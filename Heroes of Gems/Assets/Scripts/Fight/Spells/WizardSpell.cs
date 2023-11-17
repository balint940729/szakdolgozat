using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Wizard")]
public class WizardSpell : SpellBase {

    public WizardSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().getEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().getPlayerTeam();
        }

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            caster.spellAttack(caster.GetSpellDamage(), target);
        }
    }
}