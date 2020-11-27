using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KC.Common {
    public sealed class InputManager : MonoBehaviour {

        #region Instance

        public static InputManager instance { get; private set; }
        private void Awake() => instance = this;
        private void OnDestroy() => instance = null;

        #endregion

        #region Private

        private static Coroutine coroutine = default;
        private static Vector2 mouseDownPos = default;
        private static float mouseDownTime = default;

        private void Start() { coroutine = StartCoroutine(WaitForPress()); }

        private void CheckView(bool isKitchen) {
            if (isKitchen) {
                if (coroutine == null) { coroutine = StartCoroutine(WaitForPress()); }
            } else {
                if (coroutine != null) {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
            }
        }
        private void InvokeNormalizedSwipe(Vector2 direction, UnityAction<Swipe> onSwipe) {
            if (direction.magnitude == 0) { return; }
            onSwipe?.Invoke(
                Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ?
                direction.x > 0 ? Swipe.Right : Swipe.Left
                : direction.y > 0 ? Swipe.Up : Swipe.Down
                );
        }

        private IEnumerator WaitForPress() {
            yield return null;
            while (true) {
                if (Input.GetMouseButtonDown(0)) {
                    if (UI.instance.common.KitchenLook) {
                        Actions.Kitchen.OnPress?.Invoke(MouseWorldPosition);
                    } else {
                        Actions.Restaurant.OnPress?.Invoke(MouseWorldPosition);
                    }
                    mouseDownPos = Input.mousePosition;
                    mouseDownTime = Time.time;
                    coroutine = StartCoroutine(WaitForRelease());
                    break;
                }
                yield return null;
            }
        }
        private IEnumerator WaitForRelease() {
            while (true) {
                if (Input.GetMouseButtonUp(0)) {
                    var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var localDir = (Vector2)Input.mousePosition - mouseDownPos;
                    if (Time.time - mouseDownTime < 0.05f || localDir.magnitude < 30) {
                        if (UI.instance.common.KitchenLook) {
                            Actions.Kitchen.OnRelease?.Invoke(mouseWorldPos);
                            Actions.Kitchen.OnTap?.Invoke(mouseWorldPos);
                        } else {
                            Actions.Restaurant.OnRelease?.Invoke(mouseWorldPos);
                            Actions.Restaurant.OnTap?.Invoke(mouseWorldPos);
                        }
                    } else {
                        if (UI.instance.common.KitchenLook) {
                            Actions.Kitchen.OnRelease?.Invoke(mouseWorldPos);
                            InvokeNormalizedSwipe(localDir, Actions.Kitchen.OnSwipe);
                        } else {
                            Actions.Restaurant.OnRelease?.Invoke(mouseWorldPos);
                            Actions.Restaurant.OnSwipe?.Invoke(localDir);
                        }
                    }
                    coroutine = StartCoroutine(WaitForPress());
                    break;
                }
                yield return null;
            }
        }

        #endregion

        #region Public

        public static class Actions {
            public static class Restaurant {
                public static UnityAction<Vector2> OnSwipe = default;
                public static UnityAction<Vector2> OnTap = default;
                public static UnityAction<Vector2> OnPress = default;
                public static UnityAction<Vector2> OnRelease = default;
            }
            public static class Kitchen {
                public static UnityAction<Swipe> OnSwipe = default;
                public static UnityAction<Vector2> OnTap = default;
                public static UnityAction<Vector2> OnPress = default;
                public static UnityAction<Vector2> OnRelease = default;
            }
        }
        public static Vector2 MouseWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

        public enum Swipe { Up, Down, Left, Right }

        #endregion
    }
}
