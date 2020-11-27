using KC.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KC.Kitchen {
    public sealed class DishManager : MonoBehaviour {

        #region Instance

        public static DishManager instance { get; private set; }
        private void Awake() => instance = this;
        private void OnDestroy() => instance = null;

        #endregion

        #region Private

        [SerializeField] private Vector2 dishPos = default;
        [SerializeField] private float gap = default;

        private DishBuilder actualDish = default;

        private IEnumerator DishFinalization(Transform movable, float t, UnityAction end = null) {
            yield return StartCoroutine(Lerps.RotateLerp(movable, 180, t / 5.0f)); // todo: replace with animator
            yield return StartCoroutine(Lerps.RotateLerp(movable, 360, t / 5.0f));
            yield return new WaitForSeconds(2 * t / 5.0f);
            yield return StartCoroutine(Lerps.MoveLerp(movable, Vector2.right * Constants.SCREEN_WORLD_WIDTH, t / 5.0f));
            // todo
            end?.Invoke();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(dishPos, Vector3.one);
        }

        #endregion

        #region Public

        public void AddIngredient(List<InputManager.Swipe> swipes) {
            if (actualDish == null) {
                actualDish = DishBuilder.Create();
                actualDish.transform.position = dishPos;
            }

            var ingredient = Ingredient.Create(swipes);
            if (ingredient == null) { return; }

            actualDish.AddIngredient(ingredient, gap);
        }

        public bool CheckIfCorrect(Order order) {
            if (actualDish == null) { return false; }
            if (!Service.CompareLists(actualDish.ingredients, order.Recipe.Ingredients)) { return false; }
            return true;
        }

        public void OnDishFin(Order order) {
            // Creating instances for Restaurant
            var finDish = actualDish.Finalize();
            var orderData = order.TurnToData();
            // Fin corrections
            Destroy(actualDish);
            actualDish = null;
            // todo anim;
            StartCoroutine(DishFinalization(finDish.transform, 1,
                () => GameLogic.Restaurant.RecieveDish(finDish, orderData) // Send to Restaurant
                ));
        }

        #endregion
    }
}