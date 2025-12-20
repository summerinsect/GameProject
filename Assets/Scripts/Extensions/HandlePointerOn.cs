using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandlePointerOn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.0f);
    private Vector3 originalScale;
    public float scaleSpeed = 0.1f;

    private void Start() {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(targetScale));
    }

    public void OnPointerExit(PointerEventData eventData) {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(originalScale));
    }

    private IEnumerator ScaleOverTime(Vector3 target) {
        Vector3 currentScale = transform.localScale;
        float time = 0;
        while (time < scaleSpeed) {
            transform.localScale = Vector3.Lerp(currentScale, target, time / scaleSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = target;
    }
}
