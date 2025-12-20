using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour {
    public static ActionBar instance;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public ActionBarSlot[] actionBarSlots;
    public Dictionary<ActionBarSlot, int> oldRank = new Dictionary<ActionBarSlot, int>();
    public Dictionary<ActionBarSlot, Vector2> startPosition = new Dictionary<ActionBarSlot, Vector2>();
    public Dictionary<ActionBarSlot, Vector2> targetPosition = new Dictionary<ActionBarSlot, Vector2>();

    private int StringToInt(string s) {
        int n = 0;
        for (int i = 0; i < s.Length; i++) {
            n = n * 10 + (s[i] - '0');
        }
        return n;
    }
    public void InitActionBar() {
        BattleManager.instance.OnCharacterDied += UpdateActionBar;
        actionBarSlots = GetComponentsInChildren<ActionBarSlot>();
        for (int i = 0; i < actionBarSlots.Length; i++) {
            actionBarSlots[i].gameObject.SetActive(true);
            actionBarSlots[i].ClearSlotUI();
        }
        int count = 0;
        foreach (var character in BattleManager.instance.GetAllTeamMember(0))
            actionBarSlots[count++].SetCharacter(character);
        foreach (var character in BattleManager.instance.GetAllTeamMember(1))
            actionBarSlots[count++].SetCharacter(character);
        for (int i = count; i < actionBarSlots.Length; i++)
            actionBarSlots[i].gameObject.SetActive(false);
        ComputeSlotsRank();
        foreach (ActionBarSlot slot in actionBarSlots) {
            if (slot.isActiveAndEnabled == false)
                continue;
            slot.rectTransform.anchoredPosition = new Vector2(0, -slot.rank * 120);
            slot.UpdateSlotUI();
        }
    }

    private void ComputeSlotsRank() {
        foreach (ActionBarSlot sloti in actionBarSlots) {
            if (sloti.isActiveAndEnabled == false)
                continue;
            sloti.rank = 0;
            foreach (ActionBarSlot slotj in actionBarSlots) {
                if (slotj.isActiveAndEnabled == false)
                    continue;
                int uidi = StringToInt(sloti.character.uid);
                int uidj = StringToInt(slotj.character.uid);
                int ti = sloti.character.nextRoundTime;
                int tj = slotj.character.nextRoundTime;
                if (tj < ti || tj == ti && uidj < uidi)
                    sloti.rank++;
            }
        }
    }


    public Character GetNextMember() {
        foreach (ActionBarSlot slot in actionBarSlots) {
            if (slot.isActiveAndEnabled == false)
                continue;
            if (slot.rank == 0) {
                return slot.character;
            }
        }
        return null; // should not happen
    }

    public void UpdateActionBar() {
        foreach (ActionBarSlot slot in actionBarSlots) {
            if (slot.isActiveAndEnabled)
                if (slot.character.isAlive == false)
                    slot.gameObject.SetActive(false);
        }
        oldRank.Clear();
        foreach (ActionBarSlot slot in actionBarSlots) {
            if (slot.isActiveAndEnabled)
                oldRank[slot] = slot.rank;
        }
        ComputeSlotsRank();
        StartCoroutine(MoveActionBar());
    }

    public void UpdateActionBar(Character _character) {
        UpdateActionBar();
    }

    public IEnumerator MoveActionBar(float timeToMove = .4f) {
        float timeElapsed = 0;
        startPosition.Clear();
        targetPosition.Clear();
        foreach (ActionBarSlot slot in actionBarSlots) {
            if (slot.isActiveAndEnabled == false)
                continue;
            startPosition[slot] = slot.rectTransform.anchoredPosition;
            targetPosition[slot] = new Vector2(0, -slot.rank * 120);
        }
        while (timeElapsed < timeToMove) {
            float t = timeElapsed / timeToMove;
            foreach (ActionBarSlot slot in actionBarSlots) {
                if (slot.isActiveAndEnabled == false)
                    continue;
                slot.rectTransform.anchoredPosition = Vector2.Lerp(startPosition[slot], targetPosition[slot], t);
                slot.UpdateSlotUI();
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        foreach (ActionBarSlot slot in actionBarSlots) {
            if (slot.isActiveAndEnabled == false)
                continue;
            slot.rectTransform.anchoredPosition = targetPosition[slot];
            slot.UpdateSlotUI();
        }
    }
}
