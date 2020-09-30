using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    public static GameLogic L { get; protected set; }
    public static KitchenLogic Kitchen { get; protected set; }
    public static RestaurantLogic Restaurant { get; protected set; }

    protected void Awake() {
        L = this;
        Kitchen = GetComponent<KitchenLogic>();
        Restaurant = GetComponent<RestaurantLogic>();
    }
}
