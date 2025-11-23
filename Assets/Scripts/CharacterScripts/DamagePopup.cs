using UnityEngine;
using TMPro; // 引入TextMeshPro命名空间

public class DamagePopup : MonoBehaviour {
    public float moveSpeed = 1f;
    public float fadeOutTime = 0.5f;
    public float disappearDelay = 0.5f;

    private TextMeshProUGUI textMesh;
    private float timer;
    private Color textColor;

    private void Awake() {
        textMesh = GetComponent<TextMeshProUGUI>();
        textColor = textMesh.color;
        timer = 0f;
    }

    public void Setup(int damageAmount) {
        textMesh.SetText(damageAmount.ToString()); 
    }

    private void Update() {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        timer += Time.deltaTime;
        if (timer > disappearDelay) {
            float fadeProgress = (timer - disappearDelay) / fadeOutTime;
            if (fadeProgress >= 1f) {
                Destroy(gameObject);
                return;
            }
            float alpha = 1f - fadeProgress;
            textColor.a = alpha;
            textMesh.color = textColor;
        }
    }
}