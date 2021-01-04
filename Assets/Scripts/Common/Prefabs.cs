using UnityEngine;

namespace KC.Common {
    public static class Prefabs {
        public static class Kitchen {
            private readonly static string path = "Prefabs/Kitchen/";
            public readonly static GameObject order = Resources.Load<GameObject>($"{path}Order");
            public static class Ingredients {
                private readonly static string path = $"{Kitchen.path}Ingredients/";
                public static Ingredient bread = Resources.Load<GameObject>($"{path}Bread").GetComponent<Ingredient>();
                public static Ingredient cheese = Resources.Load<GameObject>($"{path}Cheese").GetComponent<Ingredient>();
                public static Ingredient grass = Resources.Load<GameObject>($"{path}Grass").GetComponent<Ingredient>();
                public static Ingredient meat = Resources.Load<GameObject>($"{path}Meat").GetComponent<Ingredient>();
                public readonly static Ingredient[] All = new[] { bread, cheese, grass, meat };
            }
        }
        public static class Restaurant {
            private readonly static string path = "Prefabs/Restaurant/";
            public readonly static GameObject dish = Resources.Load<GameObject>($"{path}Dish");
            public readonly static GameObject customer = Resources.Load<GameObject>($"{path}Customer");
        }
    }
}