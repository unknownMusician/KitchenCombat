using System.Collections.Generic;
using UnityEngine;
using static GameLogic;

public class Ingredient : MonoBehaviour {

    [SerializeField] protected Sprite[] sprites = default;
    [SerializeField] protected Sprite orderIcon = default;
    [SerializeField] protected Sprite orderTextIcon = default;
    [SerializeField] protected List<InputManager.Swipe> neededCombo = default;
    [SerializeField] protected float price = default;

    public Sprite[] Sprites => sprites;
    public Sprite OrderIcon => orderIcon;
    public Sprite OrderTextIcon => orderTextIcon;
    public List<InputManager.Swipe> NeededCombo => neededCombo;
    public float Price => price;

    protected Ingredient() { }

    public static Ingredient Create(List<InputManager.Swipe> swipes) {
        foreach (var ingr in Prefabs.Kitchen.Ingredients.All) {
            if (Service.CompareLists(ingr.neededCombo, swipes)) {
                return Instantiate(ingr.gameObject).GetComponent<Ingredient>();
            }
        }
        return null;
    }

    public override bool Equals(object ingrObj) => Service.CompareLists(neededCombo, ((Ingredient)ingrObj)?.neededCombo); // todo: remove
}
