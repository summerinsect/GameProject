
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CharacterD : Character {
    public int skillCount;

    private void Start() {
        skillCount = 0;
    }

    protected override int ProcessSingleRound() {
        Vector3Int? targetId = FindTargetLogic();
        if (targetId != null) {
            AttackLogic(targetId);
            AttackFarthest();
            return 1;
        }
        else {
            MoveLogic();
            return 0;
        }
    }

    private void AttackFarthest() {
        skillCount++;
        if (skillCount == 3) {
            List<Character> enemies = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
            int maxDistance = -1;
            foreach (var enemy in enemies) {
                int distance = GridManager.instance.Distance(position, enemy.position);
                if (distance > maxDistance)
                    maxDistance = distance;
            }
            Extensions.Shuffle(enemies);
            foreach (var enemy in enemies)
                if (GridManager.instance.Distance(position, enemy.position) == maxDistance)
                    AttackLogic(enemy.position);
            skillCount = 0;
        }
    }
}
