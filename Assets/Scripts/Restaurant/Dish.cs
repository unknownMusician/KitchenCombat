using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour {
    protected Dish() { }

    public Rigidbody2D RigidbodyComponent { get; set; }

    void Awake() {
        RigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    public void Land() {
        // Physics2D.OverlapCircleAll(transform.position, 1, ); // todo
    }
}
