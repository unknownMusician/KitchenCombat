using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient {

    public string name = default;
    public Sprite[] sprites = default;
    public List<GameLogic.SwipeType> neededCombo = default;
    public float price = default;

    public Ingredient(string name, float price, GameLogic.SwipeType[] neededCombo, params Sprite[] sprite) {
        this.name = name;
        this.sprites = sprite;
        this.neededCombo = new List<GameLogic.SwipeType>(neededCombo);
        this.price = price;
    }

    public static List<Ingredient> ingredients = new List<Ingredient>(new Ingredient[] {
            Ingredients.breadBottom,
            Ingredients.meat
            // new Ingredient(new GameLogic.SwipeType[] { /* todo */ }),
            // new Ingredient(new GameLogic.SwipeType[] { /* todo */ }),
            // new Ingredient(new GameLogic.SwipeType[] { /* todo */ })
        });

    public static Ingredient GetIngredient(List<GameLogic.SwipeType> swipes) {
        foreach (var ingredient in ingredients) {
            // if(ingredient.neededCombo.Count != swipes.Count) { continue; } // todo: if combo consists of more than 3 swipes
            bool ok = true;
            for(int i = 0; i < swipes.Count; i++) {
                if(ingredient.neededCombo[i] != swipes[i]) {
                    ok = false;
                    break;
                }
            }
            if (ok) { return ingredient; }
        }
        return null;
    }

    public static class Ingredients {
        public readonly static Ingredient breadBottom = new Ingredient(
            "Bread",
            0.10f,
            new[] { GameLogic.SwipeType.Down, GameLogic.SwipeType.Down, GameLogic.SwipeType.Down },
            GameLogic.Sprites.Kitchen.breadBottom, GameLogic.Sprites.Kitchen.breadTop);
        public readonly static Ingredient meat = new Ingredient(
            "Meat",
            0.30f,
            new[] { GameLogic.SwipeType.Up, GameLogic.SwipeType.Down, GameLogic.SwipeType.Up },
            GameLogic.Sprites.Kitchen.meat);
    }
    // todo
}
