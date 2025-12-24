using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Boss : Character {
    protected override void Awake() {
        level = 3;
        characterBattleAnimator = GetComponent<CharacterBattleAnimator>();
        if (characterBattleAnimator == null)
            characterBattleAnimator = gameObject.AddComponent<CharacterBattleAnimator>();
        currentHealth = currentMaxHealth = maxHealth;
        moveInterval = Mathf.FloorToInt(20000f / speed);
    }

    public override int IsDamagedBy(int damage) {
        int shieldDecrease = Math.Min(shield, damage);
        shield -= shieldDecrease;
        damage -= shieldDecrease;
        currentHealth -= damage;
        if (!isAlive)
            ActionsWhenDie();
        characterBattleAnimator.PlayDamageEffect(damage);
        UpdateHealthUI();
        return damage;
    }

    public GameObject healthBar;
    public GameObject fillHealth;
    public GameObject fillShield;

    public override void UpdateHealthUI() {
        float totalLength = healthBar.GetComponent<RectTransform>().rect.width;
        float height = healthBar.GetComponent<RectTransform>().rect.height;
        float healthPercent = 1.0f * currentHealth / maxHealth;
        float shieldPercent = 1.0f * shield / maxHealth;
        shieldPercent = Mathf.Min(shieldPercent, 1.0f);
        fillHealth.GetComponent<RectTransform>().sizeDelta = new Vector2(healthPercent * totalLength, 0);
        fillShield.GetComponent<RectTransform>().sizeDelta = new Vector2(shieldPercent * totalLength, 0);
    }

    public override void ActionsWhenStart() {
        base.ActionsWhenStart();
        shield = Mathf.FloorToInt(0.2f * maxHealth);
        healthBar = GameObject.Find("BossHealthBar").gameObject;
        fillHealth = GameObject.Find("BossHealth").gameObject;
        fillShield = GameObject.Find("BossShield").gameObject;
    }

    int skillCount = 0;

    protected override int ProcessSingleRound() {
        RemoveEffect();
        if (skillCount >= 1) {
            shield = Mathf.FloorToInt(0.25f * maxHealth);
            UpdateHealthUI();
        }
        if (shield > 0) {
            skillCount = 0;
            attackDistance = 4;
            attackRange = 3;
        }
        else {
            attackDistance = 2;
            attackRange = 1;
            ++skillCount;
        }
        Vector3Int? targetId = FindTargetLogic();
        if (targetId != null) {
            AttackLogic(targetId);
            return 1;
        }
        else {
            MoveLogic();
            return 0;
        }
    }

    public override void AttackLogic(Vector3Int? _targetPosition, float ratio = 1) {
        if (_targetPosition == null) return;
        Vector3Int attackPosition = _targetPosition.Value;

        characterBattleAnimator.Attack(GridManager.instance.ComputeOffset(attackPosition));

        List<Character> enemies = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
        foreach (Character enemy in enemies)
            if (GridManager.instance.Distance(attackPosition, enemy.position) < attackRange) {
                string targetId = enemy.uid;
                if (shield > 0) {
                    enemy.nextRoundTime += Mathf.FloorToInt(0.2f * enemy.moveInterval);
                    ActionBar.instance.UpdateActionBar();
                }
                BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId, ratio), uid);
            }
    }
}
