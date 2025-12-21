using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {
    public static ResultManager instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameObject victory;
    public GameObject defeat;

    public GameObject time;
    public GameObject battleScore;
    public GameObject eventScore;
    public GameObject characterScore;
    public GameObject levelUpScore;
    public GameObject coinScore;
    public GameObject timeScore;
    public GameObject totalScore;

    public GameObject[] damageSlots = new GameObject[10];
    public Character[] tempCharacters = new Character[10];

    private void Start() {
        SetTitle();
        SetScores();
        SetDamageCount();
    }

    private void SetTitle() {
        if (GameManager.instance.victory) {
            victory.SetActive(true);
            defeat.SetActive(false);
        }
        else {
            victory.SetActive(false);
            defeat.SetActive(true);
        }
    }

    private void SetScores() {
        DateTime endTime = DateTime.Now;
        DateTime startTime = GameManager.instance.startTime;
        int totalSeconds = (int)(endTime - startTime).TotalSeconds;
        if (totalSeconds % 60 < 10)
            time.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = $"{totalSeconds / 60}:0{totalSeconds % 60}";
        else
            time.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = $"{totalSeconds / 60}:{totalSeconds % 60}";
        int score = 0;
        score += GameManager.instance.battleCount * 100;
        battleScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = $"{GameManager.instance.battleCount * 100}";
        score += GameManager.instance.eventCount * 50;
        eventScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = $"{GameManager.instance.eventCount * 50}";
        score += GameManager.instance.characterCount * 20;
        characterScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = $"{GameManager.instance.characterCount * 20}";
        score += GameManager.instance.levelUpCount * 30;
        levelUpScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = $"{GameManager.instance.levelUpCount * 30}";
        score += GameManager.instance.coinCount;
        coinScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = $"{GameManager.instance.coinCount}";
        if (GameManager.instance.victory) {
            if (totalSeconds <= 240) {
                score += 200;
                timeScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "光速";
                timeScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = "200";
            }
            else if (totalSeconds <= 360) {
                score += 100;
                timeScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "高速";
                timeScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = "100";
            }
            else {
                score += 50;
                timeScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "通关";
                timeScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = "50";
            }
        }
        else {
            timeScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            timeScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = "";
        }
        totalScore.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = $"{score}";

        // check achievements
        if (GameManager.instance.victory) {
            AchievementManager.instance.Achieve(1);
            if (totalSeconds <= 120) {
                AchievementManager.instance.Achieve(4);
            }
            if (score >= 1250) {
                AchievementManager.instance.Achieve(5);
            }
            int count3star = 0;
            foreach (var character in BagManager.instance.members) {
                if (character.level == 3) {
                    count3star++;
                }
            }
            if (count3star >= 2) {
                AchievementManager.instance.Achieve(6);
            }
            if (GameManager.instance.coinCount >= 120) {
                AchievementManager.instance.Achieve(8);
            }
            if (GameManager.instance.coinCount <= 20) {
                AchievementManager.instance.Achieve(9);
            }
            if (BagManager.instance.members.Count == GameManager.instance.characterCount) {
                AchievementManager.instance.Achieve(11);
            }
        }
        
    }

    private void SetDamageCount() {
        Dictionary<string, (string, int)> damageCount = GameManager.instance.damageCount;
        for (int i = 0; i < 10; i++) {
            tempCharacters[i] = null;
            damageSlots[i].transform.Find("Image").GetComponent<Image>().sprite = null;
            damageSlots[i].transform.Find("Image").GetComponent<Image>().color = Color.clear;
            damageSlots[i].transform.Find("DamageNumber").GetComponent<TextMeshProUGUI>().text = "";
        }
        for (int i = 0; i < 10; i++) {
            int maxDamage = 0;
            foreach (var entry in damageCount) {
                if (entry.Value.Item2 > maxDamage) {
                    maxDamage = entry.Value.Item2;
                    damageSlots[i].transform.Find("DamageNumber").GetComponent<TextMeshProUGUI>().text = $"{entry.Value.Item2}";
                }
            }
            if (maxDamage >= 10000) {
                AchievementManager.instance.Achieve(10);
            }
            if (maxDamage == 0)
                break;
            foreach (var entry in damageCount) {
                if (entry.Value.Item2 == maxDamage) {
                    Debug.Log($"{entry.Value.Item1}");
                    tempCharacters[i] = CharacterCreater.instance.CreateCharacter(entry.Value.Item1);
                    damageSlots[i].transform.Find("Image").GetComponent<Image>().sprite = tempCharacters[i].GetComponent<SpriteRenderer>().sprite;
                    damageSlots[i].transform.Find("Image").GetComponent<Image>().color = Color.white;
                    damageCount.Remove(entry.Key);
                    break;
                }
            }
        }
        
    }

    public void ReturnHome() {
        
        GameManager.instance.GameOver();
    }
}
