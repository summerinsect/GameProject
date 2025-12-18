using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleSceneManager : MonoBehaviour {
    public static UI_BattleSceneManager instance;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public Button startButton;
    public Button endButton;
    public ActionBar actionBar;
    public GameObject reward;
    public GameObject bag;
    public GameObject statsPanel;
    public GameObject bossUI;

    public TextMeshProUGUI characterCountText;
    private Coroutine flashCoroutine;

    public float gridScale;
    public void Start() {
        if (startButton != null) {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StageInputHandler.instance.StartBattle);
        }
        if (endButton != null) {
            endButton.onClick.RemoveAllListeners();
            endButton.onClick.AddListener(StageInputHandler.instance.EndBattle);
        }
        UpdateCharacterCountText();
    }

    public void ChangeUIForStart() {
        statsPanel.gameObject.SetActive(false);
        bag.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        characterCountText.gameObject.SetActive(false);
        actionBar.gameObject.SetActive(true);
        EnlargeGrid(gridScale);
    }


    public void EnlargeGrid(float scale) {
        GridManager.instance.transform.localScale *= scale;
        foreach (var character in BattleManager.instance.GetAllTeamMember(0)) {
            character.transform.localScale *= scale;
            character.transform.position = GridManager.instance.ComputeOffset(character.position);
        }
        foreach (var character in BattleManager.instance.GetAllTeamMember(1)) {
            character.transform.localScale *= scale;
            character.transform.position = GridManager.instance.ComputeOffset(character.position);
        }
    }

    public void ChangeUIForFinish(int winner) {
        EnlargeGrid(1f / gridScale);
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);
        actionBar.gameObject.SetActive(false);
        if (GameManager.instance.playerDepth == 11)
            bossUI.gameObject.SetActive(false);
        if (winner == 1) {
            endButton.GetComponentInChildren<TextMeshProUGUI>().text = "游戏结束";
            return;
        }
        reward.gameObject.SetActive(true);
        statsPanel.gameObject.SetActive(true);
        bag.gameObject.SetActive(true);
    }

    public void UpdateCharacterCountText() {
        characterCountText.text = "上场角色数：" + StageInputHandler.instance.currentCharacterCount.ToString() + "/" + StageManager.instance.maxCharacterCount.ToString();
    }

    public void StartFlashText() {
        if (flashCoroutine != null) {
            StopCoroutine(flashCoroutine);
            characterCountText.color = Color.white;
        }
        flashCoroutine = StartCoroutine(FlashText());
    }

    private IEnumerator FlashText(float duration = 1f, float interval = .1f) {
        float startTime = Time.time;
        Color originalColor = Color.white;
        Color flashColor = Color.red;
        characterCountText.gameObject.SetActive(true);
        characterCountText.color = originalColor;
        while (Time.time < startTime + duration) {
            characterCountText.color = flashColor;
            yield return new WaitForSeconds(interval);
            characterCountText.color = originalColor;
            yield return new WaitForSeconds(interval);
        }
        characterCountText.color = originalColor;
        flashCoroutine = null;
    }
}
