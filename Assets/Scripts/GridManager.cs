using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour {
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private int size;

    private void Start() {
        for (int xOffset = -size; xOffset <= size; xOffset++) {
            int countY = 2 * size + 1 - Mathf.Abs(xOffset);
            float minY = Mathf.Abs(xOffset) * .5f - size;
            for (int yOffset = 0; yOffset < countY; yOffset++) {
                Vector3 offset = new Vector3(xOffset, minY + yOffset);
                offset.x *= .75f;
                offset.y *= Mathf.Sqrt(3) * .5f;
                Instantiate(gridPrefab, transform.position + offset, Quaternion.identity, transform);
            }
        }
    }
}
