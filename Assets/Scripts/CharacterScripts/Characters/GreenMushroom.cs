using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMushroom : Character {

    public override void ActionsWhenStart() {
        base.ActionsWhenStart();
        BattleManager.instance.attackedByGreen.Clear();
    }

    public override void AttackLogic(Vector3Int? _targetPosition, float ratio = 1) {
        if (_targetPosition == null) return;
        Vector3Int attackPosition = _targetPosition.Value;

        characterBattleAnimator.Attack(GridManager.instance.ComputeOffset(attackPosition));

        List<Character> enemies = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
        foreach (Character enemy in enemies)
            if (GridManager.instance.Distance(attackPosition, enemy.position) < attackRange) {
                string targetId = enemy.uid;
                enemy.ModifyDamage(-0.2f, 2);
                if (BattleManager.instance.attackedByRed.ContainsKey(targetId)) {
                    BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId, ratio + 1f));
                    BattleManager.instance.attackedByRed.Remove(targetId);
                }
                else {
                    BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId, ratio));
                    BattleManager.instance.attackedByGreen[targetId] = enemy;
                }
            }
    }
}
