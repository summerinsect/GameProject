using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainSceneManager : MonoBehaviour {
    public void StartGame() {
        GameManager.instance.StartGame();
    }

    public GameObject rules;
    public GameObject achievements;

    public void ShowRules() {
        rules.SetActive(true);
    }

    public void HideRules() {
        rules.SetActive(false);
    }

    public void ShowAchievements() {
        achievements.SetActive(true);
    }

    public void HideAchievements() {
        achievements.SetActive(false);
    }   

    public void QuitGame() {
        // 1. 让编译后的正式版游戏退出
        Application.Quit();

        // 2. 让在编辑器模式下的预览停止（方便测试）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Debug.Log("游戏已退出");
    }
}
