using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {

    [SerializeField]
    private bool isFree;

    public bool getIsFree() {
        return this.isFree
    }

    public void setIfFree(bool isFree) {
        this.isFree = isFree;
    }

}
