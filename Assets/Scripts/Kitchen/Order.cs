using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {
    public Recipe recipe = default;
    public float price = default;
    public float timeToExpire = default;
    public int tableId = default;

    private Order() { }
    public static Order Create(Recipe recipe, float price, float timeToExpire, int tableId) {
        var order = Instantiate(GameLogic.Prefabs.order).GetComponent<Order>();
        order.recipe = recipe;
        order.price = price;
        order.timeToExpire = timeToExpire;
        order.tableId = tableId;
        return order;
    }
}
