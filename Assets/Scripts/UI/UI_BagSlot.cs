using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_BagSlot : MonoBehaviour, IPointerDownHandler {
    public Character characterScript;
    public Image image;

    public void Start() {
        image = GetComponent<Image>();
    }

    public void UpdateSlot(string _character) {
        characterScript = CharacterCreater.instance.CreateCharacter(_character, 0, new Vector3Int(0, 0, 0));
        SpriteRenderer sr = characterScript.GetComponent<SpriteRenderer>();
        image.sprite = sr.sprite;
        image.color = sr.color;
    }

    public void CleanUpSlot() {
        characterScript = null;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Choose character - " + characterScript.characterName);
    }
}
