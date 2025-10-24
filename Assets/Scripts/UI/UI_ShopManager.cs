using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShopManager : MonoBehaviour {
    public static UI_ShopManager instance { get; private set; }
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    [SerializeField] public GameObject shop;
    public UI_ShopSlot[] shopSlots;

    public void InitShopUI() {
        shopSlots = shop.GetComponentsInChildren<UI_ShopSlot>();
        UpdateSlotUI();
    }

    public void UpdateSlotUI() {
        for (int i = 0; i < shopSlots.Length; i++)
            shopSlots[i].CleanUpSlot();
        List<Character> members = ShopManager.instance.shopCharacter;
        for (int i = 0; i < members.Count; i++)
            shopSlots[i].UpdateSlot(members[i]);
    }
}
