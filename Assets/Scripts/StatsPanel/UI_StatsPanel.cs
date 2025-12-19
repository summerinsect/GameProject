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

    public TextMeshProUGUI characterName;
    public Image characterImage;
    public GameObject middleStar;
    public GameObject leftStar;
    public GameObject rightStar;
    public TextMeshProUGUI health;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI attackDistance;
    public TextMeshProUGUI attackRange;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI skillDescription;

    private void Start() {
        Clear();
    }

    public void ShowStats(Character character) {
        characterName.text = character.characterName;
        characterImage.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characterImage.color = character.GetComponent<SpriteRenderer>().color;
        switch (character.level) {
            case 1:
                middleStar.SetActive(true);
                leftStar.SetActive(false);
                rightStar.SetActive(false);
                break;
            case 2:
                middleStar.SetActive(false);
                leftStar.SetActive(true);
                rightStar.SetActive(true);
                break;
            case 3:
                middleStar.SetActive(true);
                leftStar.SetActive(true);
                rightStar.SetActive(true);
                break;
            default:
                Debug.LogError("Character level exceeds maximum level of 3");
                break;
        }
        health.text = character.currentHealth.ToString() + " / " + character.maxHealth.ToString();
        attack.text = character.attack.ToString();
        attackDistance.text = character.attackDistance.ToString();
        attackRange.text = character.attackRange.ToString();
        if (character.attackRange == -1)
            attackRange.text = "特殊";
        speed.text = character.speed.ToString();
        skillDescription.text = character.skillDescription;
    }

    public void Clear() {
        characterName.text = "点击角色查看信息";
        characterImage.sprite = null;
        characterImage.color = Color.clear;
        middleStar.SetActive(false);
        leftStar.SetActive(false);
        rightStar.SetActive(false);
        health.text = "";
        attack.text = "";
        attackDistance.text = "";
        attackRange.text = "";
        speed.text = "";
        skillDescription.text = "";
    }
}
