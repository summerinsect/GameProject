using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour {
    public static GridManager instance { get; private set; }
    [SerializeField] private GameObject gridPrefab;
    public int size;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this);
        }
        else {
            instance = this;
        }
    }

    public bool CheckPosition(Vector3Int coordinate) {
        int x = coordinate.x, y = coordinate.y, z = coordinate.z;
        if (x + y + z != 0) return false;
        if (x < -size || x > size) return false;
        if (y < -size || y > size) return false;
        if (z < -size || z > size) return false;
        return true;
    }

    public Vector3 ComputeOffset(Vector3Int coordinate) {
        Debug.Assert(CheckPosition(coordinate), "Invalid position!");
        int x = coordinate.x, y = coordinate.y, z = coordinate.z;
        float xOffset = x * .75f;
        float yOffset = (y - z) * Mathf.Sqrt(3) / 4f;
        return new Vector3(xOffset, yOffset);
    }

    private void Start() {
        for (int x = -size; x <= size; x++)
            for (int y = -size; y <= size; y++)
                if (CheckPosition(new Vector3Int(x, y, - x - y)))
                    Instantiate(gridPrefab, transform.position + ComputeOffset(new Vector3Int(x, y, - x - y)), Quaternion.identity, transform);
    }
}
