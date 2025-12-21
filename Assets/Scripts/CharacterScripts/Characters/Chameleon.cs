
using UnityEngine;

public class Chameleon : Character {
    public int skillCount;

    public override void ActionsWhenStart() {
        base.ActionsWhenStart();
        skillCount = 0;
    }

    protected override int ProcessSingleRound() {
        RemoveEffect();
        Vector3Int? targetId = FindTargetLogic();
        if (targetId != null) {
            AttackLogic(targetId);
            ModifyDamage(0.15f, 10000);
            ++skillCount;
            return 1;
        }
        else {
            MoveLogic();
            for (int i = 0; i < skillCount; i++) {
                damageModifier.Remove(0.15f);
                effectToRemove.Remove((10000 - i, 2, 0.15f));
            }
            ReCalculate();
            skillCount = 0;
            return 0;
        }
    }
}