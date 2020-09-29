using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    public static KitchenLogic kitchen;
    public static RestaurantLogic restaurant;

    protected void Awake() {
        kitchen = GetComponent<KitchenLogic>();
        restaurant = GetComponent<RestaurantLogic>();
    }
}
