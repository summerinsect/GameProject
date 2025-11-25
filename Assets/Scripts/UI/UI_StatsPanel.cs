using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatsPanel : MonoBehaviour {
    public static UI_StatsPanel instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        instance = this;
    }

    [SerializeField] public TextMeshProUGUI characterName;
    [SerializeField] public Image characterImage;
    [SerializeField] public TextMeshProUGUI health;
    [SerializeField] public TextMeshProUGUI attack;
    [SerializeField] public TextMeshProUGUI attackDistance;
    [SerializeField] public TextMeshProUGUI attackRange;
    [SerializeField] public TextMeshProUGUI speed;
    [SerializeField] public TextMeshProUGUI skillDescription;

    private void Start() {
        Clear();
    }

    public void ShowStats(Character character) {
        characterName.text = character.characterName;
        characterImage.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characterImage.color = character.GetComponent<SpriteRenderer>().color;
        health.text = character.currentHealth.ToString() + " / " + character.maxHealth.ToString();
        attack.text = character.attack.ToString();
        attackDistance.text = character.attackDistance.ToString();
        attackRange.text = character.attackRange.ToString();
        speed.text = character.speed.ToString();
        skillDescription.text = character.skillDescription;
    }

    public void Clear() {
        characterName.text = "点击角色查看信息";
        characterImage.sprite = null;
        characterImage.color = Color.clear;
        health.text = "";
        attack.text = "";
        attackDistance.text = "";
        attackRange.text = "";
        speed.text = "";
        skillDescription.text = "";
    }
}
