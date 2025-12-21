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
    private bool[] achievementStatus = new bool[14];

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
        // To do: show info
        Set(index, true);
        SaveData();
    }

    public GameObject[] achievementSlots = new GameObject[14];

    public void UpdateAchievementUI() {
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
}