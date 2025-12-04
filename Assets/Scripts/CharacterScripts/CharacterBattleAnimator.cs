using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleAnimator : MonoBehaviour
{
	[Header("Movement Animation")]
	[SerializeField] private float moveSpeed;
	private Vector3 targetWorldPosition;
	private Vector3 startWorldPosition;
	private float moveProgress;
	private bool isMoving;

	[Header("Damage Effect")]
	[SerializeField] private float damageBlinkDuration;
	[SerializeField] private int damageBlinkCount;
	private SpriteRenderer spriteRenderer;
	private Canvas healthBar;
	private bool isBlinking;
	private float blinkTimer;
    public GameObject damagePopupPrefab;

	private Character character;

	public bool IsMoving => isMoving;
	public bool enableBattleAnimation;

    private void Awake()
	{
		damageBlinkDuration = 0.2f;
		damageBlinkCount = 2;
		moveSpeed = 2f;
		spriteRenderer = GetComponent<SpriteRenderer>();
		damagePopupPrefab = Resources.Load<GameObject>("DamagePopup");
		character = GetComponent<Character>();
		healthBar = GetComponentInChildren<Canvas>();
        enableBattleAnimation = false;
	}

	private void Update()
	{
		if (isMoving)
			UpdateMoveAnimation();
		if (isBlinking)
			UpdateDamageBlinkEffect();
	}


	public void EnableBattleAnimation()
	{
		enableBattleAnimation = true;
		spriteRenderer.enabled = true;
		healthBar.enabled = true;
        TeleportToPosition();
		Debug.Log($"[{gameObject.name}] Battle animation enabled, sprite shown");
	}

	public void DisableBattleAnimation()
	{
		enableBattleAnimation = false;
		spriteRenderer.enabled = false;
		healthBar.enabled = false;
        Debug.Log($"[{gameObject.name}] Battle animation disabled, sprite hidden");
	}

	public void StartMoveTo(Vector3 targetPosition)
	{
		startWorldPosition = transform.position;
		targetWorldPosition = targetPosition;
		moveProgress = 0f;
		isMoving = true;

		Debug.Log($"[{gameObject.name}] Start moving from {startWorldPosition} to {targetWorldPosition}, distance = {Vector3.Distance(startWorldPosition, targetWorldPosition)}");
	}

	public void TeleportTo(Vector3 position)
	{
		transform.position = position;
		isMoving = false;
		moveProgress = 0f;
	}
	public void TeleportToPosition()
	{
		TeleportTo(GridManager.instance.ComputeOffset(character.position));
	}

	private void UpdateMoveAnimation()
	{
		moveProgress += Time.deltaTime * moveSpeed;
		moveProgress = Mathf.Clamp01(moveProgress);
		float easedProgress = Mathf.SmoothStep(0f, 1f, moveProgress);
		transform.position = Vector3.Lerp(startWorldPosition, targetWorldPosition, easedProgress);

		if (moveProgress >= 1f)
		{
			transform.position = targetWorldPosition;
			isMoving = false;
			moveProgress = 0f;
			Debug.Log($"[{gameObject.name}] Movement complete at {targetWorldPosition}");
		}
	}

	public void PlayDamageEffect(int damage)
	{
		isBlinking = true;
		blinkTimer = 0f;
		if (damage == 0)
			return;
		CreateDamagePopup(damage);
	}

	private void UpdateDamageBlinkEffect()
	{
		blinkTimer += Time.deltaTime;

		float blinkInterval = damageBlinkDuration / (damageBlinkCount * 2);
		int blinkPhase = Mathf.FloorToInt(blinkTimer / blinkInterval);

		bool shouldShow = (blinkPhase % 2) == 0;
		spriteRenderer.enabled = shouldShow;

		if (blinkTimer >= damageBlinkDuration)
		{
			isBlinking = false;

			if (!character.isAlive)
			{
				DisableBattleAnimation();
				Debug.Log($"[{gameObject.name}] Character died, sprite hidden");
			}
		}
	}
	public bool IsMovementComplete()
	{
		return !IsMoving;
	}

    private void CreateDamagePopup(int damage) {
        if (damagePopupPrefab == null) {
            Debug.LogError("Cannot find Damage Popup Prefab!");
            return;
        }

        Vector3 spawnPosition = transform.position;
        float offsetX = Random.Range(-0.4f, 0.4f);
        float offsetY = Random.Range(0f, 0.5f);
        spawnPosition += new Vector3(offsetX, offsetY, 0);

		GameObject canvas = GameObject.Find("Canvas");
        GameObject popupObject = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity, canvas.transform);
        DamagePopup popupScript = popupObject.GetComponent<DamagePopup>();
        popupScript.Setup(damage);
    }

	public float projectileLifetime = 0.3f;

    public void Attack(Vector3 targetPosition, int projectileType = 1) {
        StartCoroutine(MoveAtAttack(targetPosition - transform.position));
        CreateProjectile(transform.position, targetPosition, projectileType);
    }

    public void CreateProjectile(Vector3 startPosition, Vector3 targetPosition, int projectileType = 1) {
        GameObject newProjectile = Instantiate(Resources.Load<GameObject>($"Projectile{projectileType}"), startPosition, Quaternion.identity);
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        float rotationAngle = Mathf.Atan2(targetPosition.y - startPosition.y, targetPosition.x - startPosition.x) * Mathf.Rad2Deg;
        newProjectile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        projectileScript.lifetime = projectileLifetime;
        projectileScript.velocity = (targetPosition - startPosition) / projectileLifetime;
    }


    public float moveAttackDuration = .1f;
	public float moveAttackSpeed = 3f;

	public IEnumerator MoveAtAttack(Vector3 direction) {
		float timeElapsed = 0f;
		direction = direction.normalized;
		while (timeElapsed < moveAttackDuration) {
			transform.position += direction * moveAttackSpeed * Time.deltaTime;
			timeElapsed += Time.deltaTime;
            yield return null;
		}
		timeElapsed = 0f;
		while (timeElapsed < moveAttackDuration) {
			transform.position -= direction * moveAttackSpeed * Time.deltaTime;
			timeElapsed += Time.deltaTime;
			yield return null;
		}
	}
}
