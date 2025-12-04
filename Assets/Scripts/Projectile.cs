using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float lifetime;
    public Vector3 velocity;

    private void Update() {
        transform.position += velocity * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }
}
