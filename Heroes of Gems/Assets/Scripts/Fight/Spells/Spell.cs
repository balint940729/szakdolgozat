using UnityEngine;

//[CreateAssetMenu(fileName = "Spell")]
public class Spell : SpellBase {

    public Spell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        //List<GameObject> targetsGO = new List<GameObject>();
        //if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
        //    targetsGO = TurnBase.GetInstance().GetEnemyTeam();
        //}
        //else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
        //    targetsGO = TurnBase.GetInstance().GetPlayerTeam();
        //}
        //UnitController target = targetsGO.First().GetComponent<UnitController>();
        //caster.SpellAttack(caster.GetSpellDamage(), target);
    }
}