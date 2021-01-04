using KC.Common;
using UnityEngine;

/// todo:
/// - add animator
/// - anim adding ingredient
/// - invent more ingredients
/// 
namespace KC.Kitchen {
    public sealed class KitchenLogic : MonoBehaviour {

        public Transform GameMenu { get; set; } = default;
        public RectTransform UIMenu { get; set; } = default;

        public bool ReceiveOrder(Order order) {
            OrderManager.instance.AddOrder(order);
            return true;
        }

        public void TEST() { // todo: TEST
            ReceiveOrder(Order.Create(new Recipe(
                Prefabs.Kitchen.Ingredients.bread, Prefabs.Kitchen.Ingredients.meat, Prefabs.Kitchen.Ingredients.bread
                ), 10, 1));
        }
    }
}