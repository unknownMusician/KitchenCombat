using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishBuilder : MonoBehaviour {

    [HideInInspector] public List<Ingredient> ingredients = new List<Ingredient>();

    protected DishBuilder() { }
    public static DishBuilder Create() => Instantiate(GameLogic.Prefabs.Kitchen.dishBuilder).GetComponent<DishBuilder>();
    public void AddIngredient(Ingredient ingredient, float gap) {
        ingredients.Add(ingredient);

        // last ingredient
        if (ingredients.Count > 1) {
            var lastingr = ingredients[ingredients.Count - 2];
            if (lastingr.Sprites.Length > 1) {
                lastingr.GetComponent<SpriteRenderer>().sprite = lastingr.Sprites[1];
            }
        }
        // current ingredient
        var ingrTransform = ingredient.transform;
        ingrTransform.SetParent(transform);
        ingrTransform.localPosition = Vector2.up * (transform.childCount - 1) * gap;
        var ingrSR = ingredient.GetComponent<SpriteRenderer>();
        ingrSR.sortingOrder = 21 + transform.childCount;
        if (ingredient.Sprites.Length > 1) {
            ingrSR.sprite = ingredient.Sprites[0];
        }
        // todo: check if last ingredient;
        // todo: spawn
        // todo: add a rule so you can only start with bread
        // todo: show or do nt show a shadow
    }
    public Dish Finalize() {
        var dish = Instantiate(GameLogic.Prefabs.Restaurant.dish).GetComponent<Dish>(); // todo: move to Dish.cs
        transform.SetParent(dish.transform);
        transform.localPosition = Vector2.zero;
        return dish;
    }
}
