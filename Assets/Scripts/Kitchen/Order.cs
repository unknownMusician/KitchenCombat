using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {
    public Recipe recipe = default;
    public float timeToExpire = default;
    public int tableId = default;

    private Order() { }
    public static Order Create(Recipe recipe, float timeToExpire, int tableId) {
        var order = Instantiate(GameLogic.Prefabs.order).GetComponent<Order>();
        order.recipe = recipe;
        order.timeToExpire = timeToExpire;
        order.tableId = tableId;
        return order;
    }

    public class Recipe {
        public List<Ingredient> ingredients;
        public float price;

        public Recipe(List<Ingredient> ingredients, float price) {
            this.ingredients = ingredients;
            this.price = price;
        }

        public static Recipe GenerateRecipe() {
            // todo: generating algorythm
            var ingrs = new List<Ingredient>();
            float price = 0;
            /*
            if (level == 4) {
                ingrs.Add(Ingredient.ingredients[2]);
                price += ...
            }else if (...) {
                // todo
            }*/
            return new Recipe(ingrs, price);
        }
    }

    public class Ingredient {

        public static List<Ingredient> ingredients = new List<Ingredient>(new Ingredient[] {
            new Ingredient(),
            new Ingredient(),
            new Ingredient(),
            new Ingredient()
        });
        // todo
    }
}
