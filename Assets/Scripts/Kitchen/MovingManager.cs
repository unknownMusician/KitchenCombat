using KC.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KC.Kitchen {
    public sealed class MovingManager : MonoBehaviour {

        #region Instance

        public static MovingManager instance { get; private set; }
        private void Awake() => instance = this;
        private void OnDestroy() => instance = null;

        #endregion

        #region Private

        private OrderManager.OrderHolder controlledOrderHolder = null;

        private void OnEnable() {
            InputManager.Actions.Kitchen.OnPress += CheckOrderPress;
            InputManager.Actions.Kitchen.OnRelease += CheckOrderRelease;
        }

        private void CheckOrderPress(Vector2 mouseWorldPosition) {
            var orderHolder = OrderManager.instance.GetOrder(mouseWorldPosition);
            if (orderHolder == null) { return; }

            ComboManager.instance.SkipNext();
            controlledOrderHolder = orderHolder;
            if (orderHolder.coroutine != null) {
                orderHolder.coroutineAuthor.StopCoroutine(orderHolder.coroutine);
                orderHolder.coroutine = null;
            }
            orderHolder.coroutineAuthor = this;
            orderHolder.coroutine = StartCoroutine(ControllOrder(orderHolder.order));
        }
        private void CheckOrderRelease(Vector2 mouseWorldPosition) {
            if (controlledOrderHolder != null) {
                controlledOrderHolder.coroutineAuthor.StopCoroutine(controlledOrderHolder.coroutine);
                controlledOrderHolder.coroutine = null;
                // If dish created correctly
                var order = controlledOrderHolder.order;
                if (DishManager.instance.CheckIfCorrect(order)) {
                    OrderManager.instance.RemoveOrder(controlledOrderHolder);
                    controlledOrderHolder = null;
                    DishManager.instance.OnDishFin(order);
                    return;
                }
                // If not
                controlledOrderHolder.coroutineAuthor = this;
                controlledOrderHolder.coroutine = StartCoroutine(
                    Lerps.MoveLerp(controlledOrderHolder.order.transform, controlledOrderHolder.regularPos, 0.2f, Lerps.Normalizators.Squared)
                    );
                controlledOrderHolder = null;
            }
        }

        private IEnumerator ControllOrder(Order order) {
            while (true) {
                yield return new WaitForFixedUpdate();
                order.transform.position = InputManager.MouseWorldPosition;
            }
        }

        #endregion
    }
}