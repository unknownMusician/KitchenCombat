using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    // Todo-list:
    // - Full Customer-Order-Dish cycle;
    // - + UI ↑↑↑
    // - Day cycle
    // - After day conclusion (receipt)
    // - Shop
    // - Dishes + Ingredients
    // - Customers
    // - ...
    // - Main Menu
    // - Guide
    // - Story
    // - ...
    // - Application build
    // - Monetization
    // - Google Play upload

    [SerializeField] protected Transform kitchenGameMenu = default;
    [SerializeField] protected Transform restaurantGameMenu = default;
    [SerializeField] protected RectTransform kitchenUIMenu = default;
    [SerializeField] protected RectTransform restaurantUIMenu = default;

    public static GameLogic L { get; protected set; }
    public static KitchenLogic Kitchen { get; protected set; }
    public static RestaurantLogic Restaurant { get; protected set; }

    public Action<SwipeType> OnSwipe;
    public Action OnTap;

    protected Vector2 mouseDownPos;
    protected float mouseDownTime;

    protected void Awake() {
        L = this;
        Kitchen = GetComponent<KitchenLogic>();
        Kitchen.GameMenu = kitchenGameMenu;
        Kitchen.UIMenu = kitchenUIMenu;
        Restaurant = GetComponent<RestaurantLogic>();
        Restaurant.GameMenu = restaurantGameMenu;
        Restaurant.UIMenu = restaurantUIMenu;
    }
    protected void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mouseDownPos = Input.mousePosition;
            mouseDownTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0)) {
            var localDir = (Vector2)Input.mousePosition - mouseDownPos;
            if (Time.time - mouseDownTime < 0.05f || localDir.magnitude < 30)
                OnTap?.Invoke();
            else
                Swipe(localDir); // todo: Даня, вот твой свайп - localDir;
        }
    }
    protected void Swipe(Vector2 direction) {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x == 0)
                return;
            else if (direction.x > 0)
                OnSwipe?.Invoke(SwipeType.Right);
            else
                OnSwipe?.Invoke(SwipeType.Left);
        } else {
            if (direction.y == 0)
                return;
            else if (direction.y > 0)
                OnSwipe?.Invoke(SwipeType.Up);
            else
                OnSwipe?.Invoke(SwipeType.Down);
        }
    }
    public enum SwipeType { Up, Down, Left, Right }
    public class Prefabs {
        public readonly static GameObject order = Resources.Load<GameObject>("Prefabs/Order");
    }
    public class Constants {
        public readonly static float SCREEN_WORLD_WIDTH = 9;
    }
}
