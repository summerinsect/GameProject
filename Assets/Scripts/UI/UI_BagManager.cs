using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BagManager : MonoBehaviour {
    [SerializeField] public Transform bagSlotsParent;
    public List<string> characters;
    public UI_BagSlot[] bagSlots;

    private void Start() {
        bagSlots = bagSlotsParent.GetComponentsInChildren<UI_BagSlot>();
        UpdateSlotUI();
    }

    private void UpdateSlotUI() {
        for (int i = 0; i < bagSlots.Length; i++)
            bagSlots[i].CleanUpSlot();
        for (int i = 0; i < characters.Count; i++)
            bagSlots[i].UpdateSlot(characters[i]);
    }
}
