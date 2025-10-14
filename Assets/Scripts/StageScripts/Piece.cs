using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    private int size;
    private Vector3Int coordinate;
    private Vector3Int[] directionsArray = new Vector3Int[]{
        new Vector3Int(1, -1, 0),
        new Vector3Int(0, 1, -1),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(0, -1, 1),
        new Vector3Int(1, 0, -1)
    };
    [SerializeField] private float moveCooldown;
    private float moveTimer;


    private void Start() {
        size = GridManager.instance.size;
        
    }

    private void updatePosition() {
        transform.position = GridManager.instance.ComputeOffset(coordinate);
    }

    private bool MoveOneStep(Vector3Int moveDirection) {
        if (GridManager.instance.CheckPosition(coordinate + moveDirection) ) {
            coordinate = coordinate + moveDirection;
            return true;
        }
        return false;
    }

    private void Update() {
        moveTimer -= Time.deltaTime;
        if (moveTimer < 0) {
            while (!MoveOneStep(directionsArray[Random.Range(0, 6)])) ;
            updatePosition();
            moveTimer = moveCooldown;
        }
    }
}
