using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public sealed class GameLogic : MonoBehaviour {

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

    [SerializeField] private Transform kitchenGameMenu = default;
    [SerializeField] private Transform restaurantGameMenu = default;
    [SerializeField] private RectTransform kitchenUIMenu = default;
    [SerializeField] private RectTransform restaurantUIMenu = default;

    [System.Serializable]
    public static class InputManager {
        #region Constructor & k

        private static GameLogic g;
        public static void Start(GameLogic g) {
            InputManager.g = g;
            coroutine = g.StartCoroutine(WaitForPress());
        }
        #endregion

        public static class Actions {
            public static class Restaurant {
                public static UnityAction<Vector2> OnSwipe = default;
                public static UnityAction<Vector2> OnTap = default;
                public static UnityAction<Vector2> OnPress = default;
                public static UnityAction<Vector2> OnRelease = default;
            }
            public static class Kitchen {
                public static UnityAction<Swipe> OnSwipe = default;
                public static UnityAction<Vector2> OnTap = default;
                public static UnityAction<Vector2> OnPress = default;
                public static UnityAction<Vector2> OnRelease = default;
            }
            //public static class Common {
            //    public static UnityAction<Vector2> OnPress = default;
            //    public static UnityAction<Vector2> OnRelease = default;
            //} // todo: remove
        }
        public enum Swipe { Up, Down, Left, Right }
        public static Vector2 MouseWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

        private static Coroutine coroutine = default;
        private static Vector2 mouseDownPos = default;
        private static float mouseDownTime = default;

        public static void CheckView(bool isKitchen) {
            if (isKitchen) {
                if (coroutine == null) { coroutine = g.StartCoroutine(WaitForPress()); }
            } else {
                if (coroutine != null) {
                    g.StopCoroutine(coroutine);
                    coroutine = null;
                }
            }
        }
        public static IEnumerator WaitForPress() {
            while (true) {
                if (Input.GetMouseButtonDown(0)) {
                    if (UI.instance.common.KitchenLook) {
                        Actions.Kitchen.OnPress?.Invoke(MouseWorldPosition);
                    } else {
                        Actions.Restaurant.OnPress?.Invoke(MouseWorldPosition);
                    }
                    mouseDownPos = Input.mousePosition;
                    mouseDownTime = Time.time;
                    coroutine = g.StartCoroutine(WaitForRelease());
                    break;
                }
                yield return null;
            }
        }
        public static IEnumerator WaitForRelease() {
            while (true) {
                if (Input.GetMouseButtonUp(0)) {
                    var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var localDir = (Vector2)Input.mousePosition - mouseDownPos;
                    if (Time.time - mouseDownTime < 0.05f || localDir.magnitude < 30) {
                        if (UI.instance.common.KitchenLook) {
                            Actions.Kitchen.OnRelease?.Invoke(mouseWorldPos);
                            Actions.Kitchen.OnTap?.Invoke(mouseWorldPos);
                        } else {
                            Actions.Restaurant.OnRelease?.Invoke(mouseWorldPos);
                            Actions.Restaurant.OnTap?.Invoke(mouseWorldPos);
                        }
                    } else {
                        if (UI.instance.common.KitchenLook) {
                            Actions.Kitchen.OnRelease?.Invoke(mouseWorldPos);
                            InvokeNormalizedSwipe(localDir, Actions.Kitchen.OnSwipe);
                        } else {
                            Actions.Restaurant.OnRelease?.Invoke(mouseWorldPos);
                            Actions.Restaurant.OnSwipe?.Invoke(localDir);
                        }
                    }
                    coroutine = g.StartCoroutine(WaitForPress());
                    break;
                }
                yield return null;
            }
        }

        private static void InvokeNormalizedSwipe(Vector2 direction, UnityAction<Swipe> onSwipe) { // todo: make swipes only work when in Kitchen
            if(direction.magnitude == 0) { return; }
            onSwipe?.Invoke(
                Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? 
                direction.x > 0 ? Swipe.Right : Swipe.Left 
                : direction.y > 0 ? Swipe.Up : Swipe.Down
                );
        }
    }

    public static GameLogic L { get; private set; }
    public static KitchenLogic Kitchen { get; private set; }
    public static RestaurantLogic Restaurant { get; private set; }

    private void Awake() {
        L = this;
        Kitchen = GetComponent<KitchenLogic>();
        Kitchen.GameMenu = kitchenGameMenu;
        Kitchen.UIMenu = kitchenUIMenu;
        Restaurant = GetComponent<RestaurantLogic>();
        Restaurant.GameMenu = restaurantGameMenu;
        Restaurant.UIMenu = restaurantUIMenu;
        InputManager.Start(this);
        Dish.InitializeDishes();
    }
    public static class Prefabs {
        public readonly static GameObject order = Resources.Load<GameObject>("Prefabs/Order");
        public readonly static GameObject dish = Resources.Load<GameObject>("Prefabs/Dish");
        public readonly static GameObject dishBuilder = Resources.Load<GameObject>("Prefabs/DishBuilder");
        public readonly static GameObject burger = Resources.Load<GameObject>("Prefabs/Burger");
        public readonly static GameObject customer = Resources.Load<GameObject>("Prefabs/Customer");
    }
    public static class Sprites {
        public static class Kitchen {
            public readonly static Sprite breadBottom = Resources.Load<Sprite>("Sprites/Kitchen/BreadBottom");
            public readonly static Sprite breadTop = Resources.Load<Sprite>("Sprites/Kitchen/BreadTop");
            public readonly static Sprite breadIngredientIcon = Resources.LoadAll<Sprite>("Sprites/Kitchen/IngredientIcons")[0];
            public readonly static Sprite breadIngredientTextIcon = Resources.LoadAll<Sprite>("Sprites/Kitchen/IngredientTextIcons")[0];

            public readonly static Sprite meat = Resources.Load<Sprite>("Sprites/Kitchen/Meat");
            public readonly static Sprite meatIngredientIcon = Resources.LoadAll<Sprite>("Sprites/Kitchen/IngredientIcons")[1];
            public readonly static Sprite meatIngredientTextIcon = Resources.LoadAll<Sprite>("Sprites/Kitchen/IngredientTextIcons")[1];

            public readonly static Sprite burger = Resources.Load<Sprite>("Sprites/Kitchen/Burger");
        }
    }
    public static class Constants {
        public readonly static float SCREEN_WORLD_WIDTH = 9;
    }
}
