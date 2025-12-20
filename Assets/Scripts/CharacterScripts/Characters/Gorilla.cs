
using System.Collections.Generic;
using UnityEngine;

public class Gorilla : Character {
    // »÷ÍË
    public override void AttackLogic(Vector3Int? _targetPosition, float ratio = 1.0f) {
        if (_targetPosition == null) return;
        Vector3Int attackPosition = _targetPosition.Value;
        characterBattleAnimator.Attack(GridManager.instance.ComputeOffset(attackPosition));
        List<Character> enemies = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
        foreach (Character enemy in enemies)
            if (GridManager.instance.Distance(attackPosition, enemy.position) < attackRange) {
                string targetId = enemy.uid;
                BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId, ratio), uid);
                Vector3Int nextPosition = 2 * enemy.position - position;
                if (GridManager.instance.CheckPosition(nextPosition) && !GridManager.instance.HasCharacter(nextPosition)) {
                    enemy.position = nextPosition;
                    Vector3 targetWorldPos = GridManager.instance.ComputeOffset(nextPosition);
                    enemy.characterBattleAnimator.TeleportTo(targetWorldPos);
                }
                else {
                    enemy.nextRoundTime += Mathf.FloorToInt(0.5f * enemy.moveInterval);
                    ActionBar.instance.UpdateActionBar();
                }
            }
    }
}
