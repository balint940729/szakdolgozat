using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Elf")]
public class ElfSpell : SpellBase {

    public ElfSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = new List<GameObject>();
        if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
            targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        }
        else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        }

        UnitController weakestTarget = targetsGO[0].GetComponent<UnitController>();
        int weakestStat = weakestTarget.GetArmor() + weakestTarget.GetHealth();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();

            if (weakestStat > (target.GetArmor() + target.GetHealth())) {
                weakestStat = target.GetArmor() + target.GetHealth();
                weakestTarget = target;
            }
        }

        caster.NormalDamage(caster.GetSpellDamage(), weakestTarget);
    }
}