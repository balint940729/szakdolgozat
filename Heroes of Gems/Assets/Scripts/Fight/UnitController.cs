using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour, IPointerClickHandler {
    private UnitDisplay unitCard;
    private SpellDisplay spellCard;
    private string folderPath = "Assets/Sprites/Cards";

    public event System.Action<string> onSpellDisplay;

    public event System.Action onTargetSelection;

    private string spellFolderPath = "Assets/Sprites/Spells";
    private string[] assetGuids;
    private string[] spellAssetGuids;

    private int health;
    private int armor;
    private int attack;
    private int spellDamage;
    private int mana;
    private int maxMana;
    private List<Colors> colors;
    private SpellBase spell;
    private Race race;

    private void Awake() {
        unitCard = GetComponent<UnitDisplay>();
        spellCard = GetComponent<SpellDisplay>();
        assetGuids = AssetDatabase.FindAssets("t:Unit", new string[] { folderPath });
    }

    public void setUp(int cardID, GameObject spellGO) {
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[cardID]);
        unitCard.card = AssetDatabase.LoadAssetAtPath<Unit>(assetPath);

        string typeSpell = "t:" + unitCard.card.name + "Spell";
        spellAssetGuids = AssetDatabase.FindAssets(typeSpell, new string[] { spellFolderPath });

        string spellAssetPath = AssetDatabase.GUIDToAssetPath(spellAssetGuids[0]);
        spellCard = spellGO.GetComponent<SpellDisplay>();
        spellCard.spell = AssetDatabase.LoadAssetAtPath<SpellBase>(spellAssetPath);

        health = unitCard.card.baseHealth;
        armor = unitCard.card.baseArmor;
        attack = unitCard.card.baseAttack;
        spellDamage = unitCard.card.baseSpellDamage;
        //mana = unitCard.card.maxMana;
        mana = unitCard.card.currentMana;
        maxMana = unitCard.card.maxMana;
        race = unitCard.card.race;

        unitCard.SetStats(health, armor, attack, mana);

        colors = unitCard.card.colors;
        spell = unitCard.card.spell;
        spell.ChangeSpellDescription(spellDamage);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer || BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
            onSpellDisplay?.Invoke(spellCard.name);
        }
        //else if (BattleStateHandler.GetState() == BattleState.PlayerTurn || BattleStateHandler.GetState() == BattleState.EnemyTurn) {
        //    onTargetSelection?.Invoke();
        //}
    }

    //public bool castSpell(List<UnitController> allyTargets, List<UnitController> targets) {
    public bool castSpell(List<UnitController> targets) {
        if (mana == maxMana) {
            if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
                BattleStateHandler.setState(BattleState.PlayerTurn);
            }
            else if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
                BattleStateHandler.setState(BattleState.EnemyTurn);
            }
            spell.setCaster(this);
            spell.setTargets(targets);
            //spell.setEnemyTargets(targets);
            //spell.setPlayerTargets(allyTargets);
            mana = 0;
            unitCard.SetMana(mana);
            SpellController.CloseSpell();

            if (spell.isSpellTargets()) {
                onTargetSelection?.Invoke();
            }
            spell.InitializeSpell();

            return true;
        }

        return false;
    }

    public void setUpColors(GameObject colorBG, int index) {
        unitCard.setColors(colorBG, index);
    }

    public List<Colors> getColors() {
        return colors;
    }

    public int getColorsCount() {
        return colors.Count;
    }

    public void normalDamage(int damage, UnitController target) {
        target.armor -= damage;

        if (target.armor < 0) {
            target.health += target.armor;
            target.armor = 0;
        }

        if (target.health < 0) {
            target.health = 0;
        }

        target.unitCard.SetStats(target.health, target.armor);
    }

    public void TrueDamage(int damage, UnitController target) {
        target.health -= damage;

        if (target.health < 0) {
            target.health = 0;
        }

        target.unitCard.SetStats(target.health, target.armor);
    }

    public void spellAttack(int spellDamage, UnitController target) {
        target.armor -= spellDamage;

        if (target.armor < 0) {
            target.health += target.armor;
            target.armor = 0;
        }

        if (target.health < 0) {
            target.health = 0;
        }

        target.unitCard.SetStats(target.health, target.armor);
    }

    public int gainMana(int manaAmount) {
        int remainedMana = 0;
        mana += manaAmount;

        if (mana > maxMana) {
            remainedMana = mana - maxMana;
            mana = maxMana;
        }

        unitCard.SetMana(mana);

        return remainedMana;
    }

    public bool isOnFullMana() {
        if (mana == maxMana) {
            return true;
        }

        return false;
    }

    public int GetHealth() {
        return health;
    }

    public int GetArmor() {
        return armor;
    }

    public int GetAttack() {
        return attack;
    }

    public int GetSpellDamage() {
        return spellDamage;
    }

    public Race GetRace() {
        return race;
    }

    public void GainSpellDamage(int amount) {
        spellDamage += amount;
        spell.ChangeSpellDescription(spellDamage);
    }

    public void GainAttack(int amount) {
        attack += amount;
        unitCard.SetAttack(attack);
    }

    public void GainArmor(int amount) {
        armor += amount;
        unitCard.SetArmor(armor);
    }

    public void GainHealth(int amount) {
        health += amount;
        unitCard.SetHealth(health);
    }

    public void GainStats(int healthAmount, int armorAmount, int attackAmount, int manaAmount) {
        health += healthAmount;
        armor += armorAmount;
        attack += attackAmount;
        mana += manaAmount;

        unitCard.SetStats(health, armor, attack, mana);
    }

    public void GainStats(int healthAmount, int armorAmount, int attackAmount) {
        health += healthAmount;
        armor += armorAmount;
        attack += attackAmount;

        unitCard.SetStats(health, armor, attack);
    }

    public void GainStats(int healthAmount, int armorAmount) {
        health += healthAmount;
        armor += armorAmount;

        unitCard.SetStats(health, armor);
    }
}