using System.Collections.Generic;
using UnityEngine;
using static GameLogic;

public class Ingredient {

    public string name = default;
    public Sprite[] Sprites { get; protected set; } = default;
    public Sprite OrderIcon { get; protected set; } = default;
    public Sprite OrderTextIcon { get; protected set; } = default;
    public List<InputManager.Swipe> NeededCombo { get; protected set; } = default;
    public float Price { get; protected set; } = default;

    public Ingredient(string name, float price, InputManager.Swipe[] neededCombo, Sprite orderIcon, Sprite orderTextIcon, params Sprite[] sprites) {
        this.name = name;
        this.Sprites = sprites;
        this.NeededCombo = new List<InputManager.Swipe>(neededCombo);
        this.Price = price;
        this.OrderIcon = orderIcon;
        this.OrderTextIcon = orderTextIcon;
    }

    public static Ingredient GetIngredient(List<InputManager.Swipe> swipes) {
        foreach (var ingredient in Ingredients.All) {
            // if(ingredient.neededCombo.Count != swipes.Count) { continue; } // todo: if combo consists of more than 3 swipes
            bool ok = true;
            for (int i = 0; i < swipes.Count; i++) {
                if (ingredient.NeededCombo[i] != swipes[i]) {
                    ok = false;
                    break;
                }
            }
            if (ok) { return ingredient; }
        }
        return null;
    }

    public static class Ingredients {
        public static Ingredient[] All { get; } = new[] {
            new Ingredient(
                "Bread",
                0.10f,
                new[] { InputManager.Swipe.Down, InputManager.Swipe.Down, InputManager.Swipe.Down },
                GameLogic.Sprites.Kitchen.breadIngredientIcon,
                GameLogic.Sprites.Kitchen.breadIngredientTextIcon,
                GameLogic.Sprites.Kitchen.breadBottom, GameLogic.Sprites.Kitchen.breadTop),
            new Ingredient(
                "Meat",
                0.30f,
                new[] { InputManager.Swipe.Up, InputManager.Swipe.Down, InputManager.Swipe.Up },
                GameLogic.Sprites.Kitchen.meatIngredientIcon,
                GameLogic.Sprites.Kitchen.meatIngredientTextIcon,
                GameLogic.Sprites.Kitchen.meat)
            // new Ingredient(new KitchenLogic.Swipe[] { /* todo */ }),
            // new Ingredient(new KitchenLogic.Swipe[] { /* todo */ }),
            // new Ingredient(new KitchenLogic.Swipe[] { /* todo */ })
        };
        public static Ingredient Bread => All[0];
        public static Ingredient Meat => All[1];
    }
}
