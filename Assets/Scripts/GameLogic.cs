using System.Collections;
using System.Collections.Generic;
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

        private static void InvokeNormalizedSwipe(Vector2 direction, UnityAction<Swipe> onSwipe) {
            if (direction.magnitude == 0) { return; }
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
    }
    public static class Prefabs {
        public static class Kitchen {
            private readonly static string path = "Prefabs/Kitchen/";
            public readonly static GameObject order = Resources.Load<GameObject>($"{path}Order");
            public readonly static GameObject dishBuilder = Resources.Load<GameObject>($"{path}DishBuilder");
            public static class Ingredients {
                private readonly static string path = $"{Kitchen.path}Ingredients/";
                public static Ingredient bread = Resources.Load<GameObject>($"{path}Bread").GetComponent<Ingredient>();
                public static Ingredient meat = Resources.Load<GameObject>($"{path}Meat").GetComponent<Ingredient>();
                public readonly static Ingredient[] All = new[] { bread, meat };
            }
        }
        public static class Restaurant {
            private readonly static string path = "Prefabs/Restaurant/";
            public readonly static GameObject dish = Resources.Load<GameObject>($"{path}Dish");
            public readonly static GameObject customer = Resources.Load<GameObject>($"{path}Customer");
        }
    }
    public static class Constants {
        public readonly static float SCREEN_WORLD_WIDTH = 9;
    }
    public static class Service {
        public static bool CompareLists<T>(List<T> list1, List<T> list2) {
            if (list1 == null || list2 == null) { return false; }
            if (list1.Count != list2.Count) { return false; }
            int size = list1.Count;
            for (int i = 0; i < size; i++) {
                if (!list1[i].Equals(list2[i])) { return false; }
            }
            return true;
        }
    }
}
