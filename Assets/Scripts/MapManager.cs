using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public static MapManager instance { get; private set; }
    private void Awake() // appear in all scenes
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int mapWidth;
    public int mapHeight;
    public int[] height;
    public List<int[]> vertices = new List<int[]>();
    public List<MapSlotType[]> types = new List<MapSlotType[]>();
    public List<bool[,]> edges = new List<bool[,]>(); 
    public List<MapSlot> mapSlots = new List<MapSlot>();

    public int depthNow;
    public void GenerateMap(int _mapWidth, int _mapHeight) {
        mapWidth = _mapWidth;
        mapHeight = _mapHeight;
        height = new int[mapWidth];
        for (int i = 0; i < mapWidth; i++) {
            height[i] = Random.Range(1, mapHeight + 1);
            if (i == mapWidth - 1)
                height[i] = 1;
            vertices.Add(new int[height[i]]);
            types.Add(new MapSlotType[height[i]]);
            for (int j = 0; j < height[i]; j++)
                types[i][j] = MapSlotType.Battle;
        }
        for (int i = 1; i < mapWidth; i++) {
            edges.Add(new bool[height[i - 1], height[i]]);
            int pos1 = 0, pos2 = 0;
            while (true) {
                edges[i - 1][pos1, pos2] = true; 
                if (pos1 == height[i - 1] - 1 && pos2 == height[i] - 1)
                    break;
                // Debug.Log(i - 1 + "," + pos1 + "->" + i + "," + pos2);
                if (pos1 < height[i - 1] - 1)
                    if (Random.Range(0, 100) < 50)
                        pos1++;
                if (pos2 < height[i] - 1)
                    if (Random.Range(0, 100) < 50)
                        pos2++;
            }
        }
    }
}
