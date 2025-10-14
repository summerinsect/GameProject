using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
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
    private bool isBlinking;
    private float blinkTimer;

    private Character character;

    public bool IsMoving => isMoving;
    public bool IsAnimating => isMoving || isBlinking;

    private void Awake()
    {
        damageBlinkDuration = 0.1f;
        damageBlinkCount = 2;
        moveSpeed = 2f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (isMoving)
        {
            UpdateMoveAnimation();
        }

        if (isBlinking)
        {
            UpdateDamageBlinkEffect();
        }
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

    public void PlayDamageEffect()
    {
        isBlinking = true;
        blinkTimer = 0f;
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

            if (character != null && !character.isAlive)
            {
                spriteRenderer.enabled = false;
                Debug.Log($"[{gameObject.name}] Character died, sprite hidden");
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }
    }
}
