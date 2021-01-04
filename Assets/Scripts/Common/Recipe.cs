using System.Collections.Generic;

namespace KC.Common {
    public class Recipe {
        public List<Ingredient> Ingredients { get; protected set; } = default;
        public float Price { get; protected set; } = default;

        public Recipe(params Ingredient[] ingredients) {
            Ingredients = new List<Ingredient>(ingredients);
            Price = 0;
            foreach (var ingr in ingredients) {
                this.Price += ingr.Price;
            }
        }

        private static Recipe GenerateBurger() {
            var ingrs = new List<Ingredient>();
            // todo
            return new Recipe(ingrs.ToArray());
        }

        public static Recipe GenerateRecipe() {
            // todo: generating algorythm

            /*
            if (level == 4) {
                ingrs.Add(Ingredient.ingredients[2]);
                price += ...
            }else if (...) {
                // todo
            }*/
            return GenerateBurger();
        }

        public static class Recipes {
            public readonly static Recipe burger = new Recipe(Prefabs.Kitchen.Ingredients.bread, Prefabs.Kitchen.Ingredients.meat, Prefabs.Kitchen.Ingredients.bread);
        }
    }
}