using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Extensions {
    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--) {
            int j = Random.Range(0, i + 1); // 0..i
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
}
