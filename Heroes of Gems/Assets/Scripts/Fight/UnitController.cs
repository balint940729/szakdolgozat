using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour, IPointerClickHandler {
    private UnitDisplay unitCard;
    private SpellDisplay spellCard;
    private readonly string folderPath = "Assets/Sprites/Cards";

    public event Action<string> OnSpellDisplay;

    //public event System.Action onTargetSelection;

    private string[] assetGuids;

    private int health;
    private int armor;
    private int attack;
    private int spellDamage;
    private int mana;
    private int maxMana;
    private List<Colors> colors;
    private SpellBaseSO spell;
    private Race race;

    private void Awake() {
        unitCard = GetComponent<UnitDisplay>();
        spellCard = GetComponent<SpellDisplay>();
    }

    public void SetUp(Unit card, GameObject spellGO) {
        assetGuids = AssetDatabase.FindAssets(card.name + " t:Unit", new string[] { folderPath });
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
        unitCard.card = AssetDatabase.LoadAssetAtPath<Unit>(assetPath);

        spellCard = spellGO.GetComponent<SpellDisplay>();
        spellCard.spell = unitCard.card.spell;

        health = unitCard.card.baseHealth;
        armor = unitCard.card.baseArmor;
        attack = unitCard.card.baseAttack;
        spellDamage = unitCard.card.baseSpellDamage;

        mana = unitCard.card.currentMana;

        if (unitCard.card.baseName == "Harpy") {
            mana = unitCard.card.maxMana;
        }

        maxMana = unitCard.card.maxMana;
        race = unitCard.card.race;

        unitCard.SetStats(health, armor, attack, mana);

        colors = unitCard.card.colors;

        spell = unitCard.card.spell;

        spellCard.SetSpellDescription(spellDamage);
    }

    private SpellBaseClass CreateSpell(SpellBaseSO spell) {
        SpellRef spellRef = new SpellRef(spell);

        return spellRef.GetSpell();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer || BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
            OnSpellDisplay?.Invoke(spellCard.name);
        }
        //else if (BattleStateHandler.GetState() == BattleState.PlayerTurn || BattleStateHandler.GetState() == BattleState.EnemyTurn) {
        //    onTargetSelection?.Invoke();
        //}
    }

    //public bool castSpell(List<UnitController> allyTargets, List<UnitController> targets) {
    public bool CastSpell(List<UnitController> targets) {
        if (mana == maxMana) {
            if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
                BattleStateHandler.SetState(BattleState.PlayerTurn);
            }
            else if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
                BattleStateHandler.SetState(BattleState.EnemyTurn);
            }

            SpellBaseClass spellLogic = CreateSpell(spell);

            spellLogic.SetCaster(this);
            spellLogic.SetTargets(targets);

            //spell.spellLog.SetCaster(this);
            //spell.spellLogic.SetTargets(targets);

            //spell.setEnemyTargets(targets);
            //spell.setPlayerTargets(allyTargets);
            mana = 0;
            unitCard.SetMana(mana);
            SpellController.CloseSpell();

            //if (spell.isSpellTargets()) {
            //    onTargetSelection?.Invoke();
            //}

            //spell.spellLogic.InitializeSpell();
            spellLogic.InitializeSpell();

            return true;
        }

        return false;
    }

    public void SetUpColors(GameObject colorBG, int index) {
        unitCard.SetColors(colorBG, index);
    }

    public List<Colors> GetColors() {
        return colors;
    }

    public int GetColorsCount() {
        return colors.Count;
    }

    public void SkullDamage(int damage, UnitController target) {
        damage = CalculateRaceBonusDamage(damage, target);

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

    public static void NormalDamage(int damage, UnitController target) {
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

    public static void TrueDamage(int damage, UnitController target) {
        target.health -= damage;

        target.health = target.health < 0 ? 0 : target.health;

        target.unitCard.SetStats(target.health, target.armor);
    }

    public int GainMana(int manaAmount) {
        int remainedMana = 0;
        mana += manaAmount;

        if (mana > maxMana) {
            remainedMana = mana - maxMana;
            mana = maxMana;
        }

        mana = mana < 0 ? 0 : mana;

        unitCard.SetMana(mana);

        return remainedMana;
    }

    private int CalculateRaceBonusDamage(int damage, UnitController target) {
        if (race.strongAgainst.Contains(target.GetRace())) {
            damage = (int)Math.Ceiling(damage * 1.5f);
        }

        if (race.weakAgainst.Contains(target.GetRace())) {
            damage = (int)Math.Ceiling(damage * 0.5f);
        }

        return damage;
    }

    public bool IsOnFullMana() {
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

    public void ModifySpellDamage(int amount) {
        spellDamage += amount;
        spellDamage = spellDamage < 0 ? 0 : spellDamage;
        spellCard.SetSpellDescription(spellDamage);
    }

    public void ModifyAttack(int amount) {
        attack += amount;
        attack = attack < 0 ? 0 : attack;
        unitCard.SetAttack(attack);
    }

    public void ModifyArmor(int amount) {
        armor += amount;
        armor = armor < 0 ? 0 : armor;
        unitCard.SetArmor(armor);
    }

    public void ModifyHealth(int amount) {
        health += amount;
        health = health < 0 ? 0 : health;
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