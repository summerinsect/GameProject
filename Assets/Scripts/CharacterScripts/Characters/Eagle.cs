
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Character {
    // 扇形

    private int Compare(Vector3Int pos1, Vector3Int pos2) {
        int x1 = pos1.x, y1 = pos1.y, z1 = pos1.z;
        int x2 = pos2.x, y2 = pos2.y, z2 = pos2.z;
        if (x1 == x2 && y1 == y2 && z1 == z2) return 0;
        if (x2 > x1 && y2 <= y1 && z2 <= z1) return 1; // 右下
        if (y2 < y1 && z2 >= z1 && x2 >= x1) return 2; // 正下
        if (z2 > z1 && x2 <= x1 && y2 <= y1) return 3; // 左下
        if (x2 < x1 && y2 >= y1 && z2 >= z1) return 4; // 左上
        if (y2 > y1 && z2 <= z1 && x2 <= x1) return 5; // 正上
        if (z2 < z1 && x2 >= x1 && y2 >= y1) return 6; // 右上
        return -1; // 不应该出现
    }
    public override void AttackLogic(Vector3Int? _targetPosition, float ratio = 1) {
        if (_targetPosition == null) return;
        List<Character> enemies = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
        int dir = Compare(position, _targetPosition.Value);
        foreach (Character enemy in enemies)
            if (Compare(position, enemy.position) == dir) {
                string targetId = enemy.uid;
                BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId, ratio));
            }
    }
}
