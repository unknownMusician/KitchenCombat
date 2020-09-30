using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

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
                Tap();
            else
                Swipe(localDir);
        }
    }
    protected void Swipe(Vector2 direction) {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x == 0)
                return;
            else if (direction.x > 0)
                Swipe(SwipeType.Right);
            else
                Swipe(SwipeType.Left);
        } else {
            if (direction.y == 0)
                return;
            else if (direction.y > 0)
                Swipe(SwipeType.Up);
            else
                Swipe(SwipeType.Down);
        }
    }
    protected void Swipe(SwipeType dir) => OnSwipe?.Invoke(dir);
    protected void Tap() => OnTap?.Invoke();
    public enum SwipeType { Up, Down, Left, Right }
}
