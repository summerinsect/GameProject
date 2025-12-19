using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : Character {

    public override void ActionsWhenStart() {
        base.ActionsWhenStart();
        BattleManager.instance.attackedByRed.Clear();
    }

    public override void AttackLogic(Vector3Int? _targetPosition, float ratio = 1) {
        if (_targetPosition == null) return;
        Vector3Int attackPosition = _targetPosition.Value;

        characterBattleAnimator.Attack(GridManager.instance.ComputeOffset(attackPosition));

        List<Character> enemies = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
        foreach (Character enemy in enemies)
            if (GridManager.instance.Distance(attackPosition, enemy.position) < attackRange) {
                string targetId = enemy.uid;
                enemy.ModifyAttack(-0.2f, 2);
                if (BattleManager.instance.attackedByGreen.ContainsKey(targetId)) {
                    BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId, ratio + 1f), uid);
                    BattleManager.instance.attackedByGreen.Remove(targetId);
                }
                else {
                    BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId, ratio), uid);
                    BattleManager.instance.attackedByRed[targetId] = enemy;
                }
            }
    }
}
