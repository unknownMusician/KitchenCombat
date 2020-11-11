using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour {
    private Dish() { }

    public Rigidbody2D RigidbodyComponent { get; set; }

    void Awake() {
        RigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    public void Land() {
        // Physics2D.OverlapCircleAll(transform.position, 1, ); // todo
    }

    public static Dish GetDish(List<Ingredient> ingredients) {
        foreach (var pair in Dishes) {
            var ingrs = pair.Key;
            int size = ingrs.Length;
            if (ingrs.Length != ingredients.Count) { return null; }
            for (int i = 0; i < size; i++) {
                if (ingrs[i] != ingredients[i]) { return null; }
            }
            return Instantiate(pair.Value).GetComponent<Dish>();
        }
        return null;
    }



    public static Dictionary<Ingredient[], GameObject> Dishes = default;
    public static void InitializeDishes() {
        Dishes = new Dictionary<Ingredient[], GameObject>();
        Dishes.Add(new[] { Ingredient.Ingredients.Bread, Ingredient.Ingredients.Meat, Ingredient.Ingredients.Bread }, GameLogic.Prefabs.burger);
    }
}
