using UnityEngine;

namespace KC.Common {
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

        #region Private

        #region Inspector

        [SerializeField] private Transform kitchenGameMenu = default;
        [SerializeField] private Transform restaurantGameMenu = default;
        [SerializeField] private RectTransform kitchenUIMenu = default;
        [SerializeField] private RectTransform restaurantUIMenu = default;

        #endregion

        private void Awake() {
            Kitchen = GetComponentInChildren<Kitchen.KitchenLogic>();
            Kitchen.GameMenu = kitchenGameMenu;
            Kitchen.UIMenu = kitchenUIMenu;
            Restaurant = GetComponent<RestaurantLogic>();
            Restaurant.GameMenu = restaurantGameMenu;
            Restaurant.UIMenu = restaurantUIMenu;
        }

        #endregion

        #region Public

        public static Kitchen.KitchenLogic Kitchen { get; private set; }
        public static RestaurantLogic Restaurant { get; private set; }

        #endregion
    }
}