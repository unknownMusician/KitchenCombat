using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public Rigidbody2D RigidbodyComponent { get; set; }

    void Awake()
    {
        RigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    public void Land()
    {
        // Physics2D.OverlapCircleAll(transform.position, 1, ); // todo
    }

    public static Dish GetDish(List<Ingredient> ingredients) {
        // todo: check & search
        return null;
    }
}
