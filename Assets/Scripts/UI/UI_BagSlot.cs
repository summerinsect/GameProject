using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_BagSlot : MonoBehaviour, IPointerClickHandler {
    public Character characterScript;
    public Image image;

    public void Start() {
        image = GetComponent<Image>();
    }

    public void UpdateSlot(Character _character) {
        characterScript = _character;
        SpriteRenderer sr = characterScript.GetComponent<SpriteRenderer>();
        image.sprite = sr.sprite;
        image.color = sr.color;
    }

    public void CleanUpSlot() {
        characterScript = null;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (characterScript == null)
            return;
        if (eventData.button == PointerEventData.InputButton.Left)
            StageInputHandler.instance.HandleSlotClick(characterScript);
    }
}
