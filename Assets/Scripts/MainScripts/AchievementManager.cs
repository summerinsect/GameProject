using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class AchiStatus {
    public bool[] achievementStatus = new bool[14];
}

public class AchievementManager : MonoBehaviour {
    public static AchievementManager instance { get; private set; }

    private const string achievementFileName = "achievement.json";
    public bool[] achievementStatus = new bool[14];

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 1. 从文件读取已有成就保存成bool数组
    public void ReadData() {
        string json = FileManager.ReadSaveText(achievementFileName);
        if (!string.IsNullOrEmpty(json)) {
            try {
                AchiStatus data = JsonUtility.FromJson<AchiStatus>(json);
                if (data != null && data.achievementStatus != null && data.achievementStatus.Length == achievementStatus.Length) {
                    achievementStatus = data.achievementStatus;
                }
            } catch {
                Debug.LogWarning("Failed to parse achievement data, using default.");
            }
        }
    }

    // 2. 保存成就到文件
    public void SaveData() {
        AchiStatus data = new AchiStatus { achievementStatus = achievementStatus };
        string json = JsonUtility.ToJson(data, true);
        FileManager.WriteSaveText(achievementFileName, json);
    }

    // 3. 设置成就
    private void Set(int index, bool value = true) {
        if (index < 0 || index >= achievementStatus.Length) return;
        achievementStatus[index] = value;
        SaveData();
    }

    // 4. 查询一个成就
    private bool Get(int index) {
        if (index < 0 || index >= achievementStatus.Length) return false;
        return achievementStatus[index];
    }

    // 可选：显示所有成就状态
    public void ShowData() {
        Debug.Log("Achievements: " + string.Join(",", achievementStatus));
    }

    public void Achieve(int index) {
        ReadData();
        if (Get(index)) return; // already achieved
        ShowAchievement(index);
        Set(index, true);
        SaveData();
    }

    public GameObject[] achievementSlots = new GameObject[14];

    public void UpdateAchievementUI() {
        ReadData();
        achievementSlots = UI_MainSceneManager.instance.achievementSlots;
        for (int i = 0; i < achievementSlots.Length; i++) {
            if (Get(i)) {
                achievementSlots[i].transform.Find("Image").GetComponent<Image>().sprite = AchievementData.instance.images[i];
                achievementSlots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = AchievementData.instance.achievements[i];
            }
            else {
                achievementSlots[i].transform.Find("Image").GetComponent<Image>().sprite = AchievementData.instance.locked;
                if (i < 12)
                    achievementSlots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = AchievementData.instance.achievements[i];
                else
                    achievementSlots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = AchievementData.instance.lockedText;
            }
        }
    }

    public void ResetAchievements() {
        for (int i = 0; i < achievementStatus.Length; i++) {
            achievementStatus[i] = false;
        }
        SaveData();
    }

    public GameObject achievementSlotPrefab;
    public bool isShowingAchievement = false;
    public Queue<int> achievementQueue = new Queue<int>();

    public void ShowAchievement() {
        if (achievementQueue.Count > 0 && !isShowingAchievement) {
            int index = achievementQueue.Dequeue();
            StartCoroutine(ShowAchievementCoroutine(index));
        }
    }

    public void ShowAchievement(int index) {
        if (isShowingAchievement) {
            achievementQueue.Enqueue(index);
        } else {
            StartCoroutine(ShowAchievementCoroutine(index));
        }
    }

    public IEnumerator ShowAchievementCoroutine(int index, float enterTime = 0.5f, float showTime = 3f) {
        isShowingAchievement = true;
        GameObject achievementSlot = Instantiate(achievementSlotPrefab, GameObject.Find("Canvas").transform);
        achievementSlot.transform.Find("Image").GetComponent<Image>().sprite = AchievementData.instance.images[index];
        achievementSlot.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = AchievementData.instance.achievements[index];
        achievementSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150);
        float elapsedTime = 0f;
        while (elapsedTime < enterTime) {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(-150, 0, elapsedTime / enterTime);
            achievementSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newY);
            yield return null;
        }
        achievementSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        elapsedTime = 0;
        while (elapsedTime < showTime) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < enterTime) {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(0, -150, elapsedTime / enterTime);
            achievementSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newY);
            yield return null;
        }
        Destroy(achievementSlot);
        isShowingAchievement = false;
        ShowAchievement();
    }
}