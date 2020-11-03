using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public List<Ingredient> ingredients = new List<Ingredient>();

    public Rigidbody2D RigidbodyComponent { get; set; }

    protected Dish() { }
    public static Dish Create() {
        var dish = Instantiate(GameLogic.Prefabs.dish).GetComponent<Dish>();
        return dish;
    }

    void Awake()
    {
        RigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    public void AddIngredient(Ingredient ingredient, float gap) {
        ingredients.Add(ingredient);
        // creating // todo: move to Ingredient.cs
        var ingrObject = new GameObject($"ingredient{ingredients.Count-1}: {ingredient.name}", typeof(SpriteRenderer));
        var ingrTransform = ingrObject.transform;
        ingrTransform.SetParent(transform);
        ingrTransform.localPosition = Vector2.up * (transform.childCount-1) * gap;
        var ingrSR = ingrObject.GetComponent<SpriteRenderer>();
        ingrSR.sprite = ingredient.sprites[0];
        ingrSR.sortingOrder = 12 + transform.childCount;
        if(ingredient.name == "Bread") {
            if(transform.childCount > 1) {
                ingrSR.sprite = ingredient.sprites[1];
            }
        }
        // todo: check if last ingredient;
        // todo: spawn
        // todo: add a rule so you can only start with bread
        // todo: show or do nt show a shadow
    }

    public void Land()
    {
        Physics2D.OverlapCircleAll(transform.position, 1, );
    }
}
