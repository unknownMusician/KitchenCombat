using System.Collections.Generic;

namespace KC.Common {
    public class Recipe {
        public List<Ingredient> Ingredients { get; protected set; } = default;
        public float Price { get; protected set; } = default;

        public Recipe(params Ingredient[] ingredients) {
            this.Ingredients = new List<Ingredient>(ingredients);
            this.Price = 0;
            foreach (var ingr in ingredients) {
                this.Price += ingr.Price;
            }
        }

        public static Recipe GenerateRecipe() {
            // todo: generating algorythm
            var ingrs = new List<Ingredient>();
            /*
            if (level == 4) {
                ingrs.Add(Ingredient.ingredients[2]);
                price += ...
            }else if (...) {
                // todo
            }*/
            return new Recipe(ingrs.ToArray());
        }

        public static class Recipes {
            public readonly static Recipe burger = new Recipe(Prefabs.Kitchen.Ingredients.bread, Prefabs.Kitchen.Ingredients.meat, Prefabs.Kitchen.Ingredients.bread);
        }
    }
}