using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Character : MonoBehaviour // Base class for all characters in the game
{
	[Header("Attributes")]
	public int price;
	public int level;
	public int maxHealth;
	public int attack;
	public int attackDistance; // ��������
	public int attackRange; // ������Χ
	public int speed;
	public string skillDescription;

	public int health;

	public string characterName;
	public string uid;
	public int teamId;
	public Vector3Int position;

	public CharacterBattleAnimator characterBattleAnimator;
	public UI_HealthBar healthBarUI;

	public int nextRoundTime;

	public bool isAlive => health > 0;

	protected virtual void Awake()
	{
		characterBattleAnimator = GetComponent<CharacterBattleAnimator>();
		if (characterBattleAnimator == null)
			characterBattleAnimator = gameObject.AddComponent<CharacterBattleAnimator>();
		healthBarUI = GetComponentInChildren<UI_HealthBar>();
        health = maxHealth;
    }

	public virtual void IsDamagedBy(int damage)
	{
		health -= damage;
		characterBattleAnimator.PlayDamageEffect();
		healthBarUI.UpdateHealthUI();
	}

	public virtual int SingleRound() 
	{
		Debug.Assert(isAlive, $"{characterName} is dead and cannot battle.");
		return ProcessSingleRound();
	}

	protected virtual int ProcessSingleRound() {
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

    public void MoveAnimation() {
        Vector3 targetWorldPos = GridManager.instance.ComputeOffset(position);
        characterBattleAnimator.StartMoveTo(targetWorldPos);
    }

    // ���ع������꣬Ĭ�������һ�����벻���� attackDistance �ĵ���
    protected virtual Vector3Int? FindTargetLogic() {
        List<Character> enemies = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
		Extensions.Shuffle(enemies);
        foreach (Character enemy in enemies)
            if (GridManager.instance.Distance(position, enemy.position) <= attackRange)
                return enemy.position;
        return null;
    }

	// ������ targetPosition Ϊ���ģ���Χ attackRange �ĵ���
    public virtual void AttackLogic(Vector3Int? _targetPosition) {
		if (_targetPosition == null) return;
		Vector3Int attackPosition = _targetPosition.Value;
        List<Character> enemies = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
        foreach (Character enemy in enemies)
            if (GridManager.instance.Distance(attackPosition, enemy.position) < attackRange) {
                string targetId = enemy.uid;
                Debug.Log($"{uid} attacks {targetId} for {attack} damage.");
                BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId));
            }
    }
	
	// �ƶ�һ����ʹ�þ�������ĵ��˾�����С
    public void MoveLogic() {
        List<Character> targets = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
        List<Vector3Int> validPositions = GridManager.instance.ValidPositions(position);

        int minDist = int.MaxValue;
        foreach (var pos in validPositions)
            minDist = Mathf.Min(minDist, GridManager.instance.GetMinDistInTargets(pos, targets));
        foreach (var pos in validPositions)
            if (GridManager.instance.GetMinDistInTargets(pos, targets) == minDist) {
                Debug.Log($"{uid} moves from {position} to {pos}.");
                position = pos;
                break;
            }
    }
}