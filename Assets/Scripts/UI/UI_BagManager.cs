using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BagManager : MonoBehaviour {
    [SerializeField] public Transform bagSlotsParent;
    public UI_BagSlot[] bagSlots;

    private void Start() {
        bagSlots = bagSlotsParent.GetComponentsInChildren<UI_BagSlot>();
        UpdateSlotUI();
    }

    private void UpdateSlotUI() {
        for (int i = 0; i < bagSlots.Length; i++)
            bagSlots[i].CleanUpSlot();
        List<Character> members = BagManager.instance.members;
        for (int i = 0; i < members.Count; i++)
            bagSlots[i].UpdateSlot(members[i]);
    }
}
