using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间
using System.Collections; // 引入 协程 命名空间
using UnityEngine.SceneManagement; // 引入 场景管理 命名空间

public class Fader : MonoBehaviour {
    public float fadeDuration = 1.5f;
    public Image fadePanel;
    public static Fader instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start() {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn() {
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1f);
        Color currentColor = fadePanel.color;
        float targetAlpha = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, elapsedTime / fadeDuration);
            fadePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }
        fadePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }

    /*
    public IEnumerator FadeOut() {
        fadePanel.raycastTarget = true;
        Color currentColor = fadePanel.color;
        float targetAlpha = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, elapsedTime / fadeDuration);
            fadePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }
        fadePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }

    public IEnumerator FadeOutAndLoadScene(string sceneName) {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
    }
    */

    private void OnDestroy() {
        StopAllCoroutines();
    }
}