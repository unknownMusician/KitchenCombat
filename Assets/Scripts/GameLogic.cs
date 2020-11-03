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

    protected void Awake() {
        L = this;
        Kitchen = GetComponent<KitchenLogic>();
        Kitchen.GameMenu = kitchenGameMenu;
        Kitchen.UIMenu = kitchenUIMenu;
        Restaurant = GetComponent<RestaurantLogic>();
        Restaurant.GameMenu = restaurantGameMenu;
        Restaurant.UIMenu = restaurantUIMenu;
    }
    public static class Prefabs {
        public readonly static GameObject order = Resources.Load<GameObject>("Prefabs/Order");
        public readonly static GameObject dish = Resources.Load<GameObject>("Prefabs/Dish");
        public readonly static GameObject dishBuilder = Resources.Load<GameObject>("Prefabs/DishBuilder");
        public readonly static GameObject customer = Resources.Load<GameObject>("Prefabs/Customer");
    }
    public static class Sprites {
        public static class Kitchen {
            public readonly static Sprite breadBottom = Resources.Load<Sprite>("Sprites/Kitchen/BreadBottom");
            public readonly static Sprite breadTop = Resources.Load<Sprite>("Sprites/Kitchen/BreadTop");
            public readonly static Sprite meat = Resources.Load<Sprite>("Sprites/Kitchen/Meat");
        }
    }
    public static class Constants {
        public readonly static float SCREEN_WORLD_WIDTH = 9;
    }
}
