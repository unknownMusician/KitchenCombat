using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {
    public Recipe recipe = default;
    public float timeToExpire = default;
    public int tableId = default;

    public Order(Recipe recipe, float timeToExpire, int tableId) {
        this.recipe = recipe;
        this.timeToExpire = timeToExpire;
        this.tableId = tableId;
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
