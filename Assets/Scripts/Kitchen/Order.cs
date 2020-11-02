using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {
    public Dish dish = default;
    public float timeToExpire = default;
    public int tableId = default;

    public Order(Dish dish, float timeToExpire, int tableId) {
        this.dish = dish;
        this.timeToExpire = timeToExpire;
        this.tableId = tableId;
    }

    public class Dish {
        public List<Ingredient> ingredients;
        public float price;

        public Dish(List<Ingredient> ingredients, float price) {
            this.ingredients = ingredients;
            this.price = price;
        }

        public static Dish GenerateDish() {
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
            return new Dish(ingrs, price);
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
