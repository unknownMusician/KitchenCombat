using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSpawner : MonoBehaviour {
    [SerializeField] private GameObject orderPrefab;

    public void Start() {
        StartCoroutine(TmpSpawnOrers());
    }

    protected IEnumerator TmpSpawnOrers() {
        while (true) {
            yield return new WaitForSeconds(5);
            var obj = Instantiate(orderPrefab, transform);
        }
        //yield return null;
    }
}
