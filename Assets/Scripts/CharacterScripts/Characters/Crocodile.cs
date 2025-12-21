
using System;

public class Crocodile : Character {
    int skillCount;

    public override void ActionsWhenStart() {
        base.ActionsWhenStart();
        skillCount = 0;
        BattleManager.instance.OnCharacterDied += Skill;
    }

    public void Skill(Character _character) {
        ModifyAttack(0.07f, 10000);
        ModifyHealth(0.07f, 10000);
        ++skillCount;
    }

    public override void ActionsWhenDie() {
        BattleManager.instance.OnCharacterDied -= Skill;
        base.ActionsWhenDie();
    }

    public override void ActionsWhenEnd() {
        BattleManager.instance.OnCharacterDied -= Skill;
        for (int i = 0; i < skillCount; i++) {
            attack = currentAttack;
            maxHealth = currentMaxHealth;
        }
        base.ActionsWhenEnd();
    }
}
