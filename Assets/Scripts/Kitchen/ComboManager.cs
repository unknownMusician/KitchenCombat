using KC.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KC.Kitchen {
    public sealed class ComboManager : MonoBehaviour {

        #region Instance

        public static ComboManager instance { get; private set; }
        private void Awake() => instance = this;
        private void OnDestroy() => instance = null;

        #endregion

        #region Private

        private bool skipNext;

        private void Start() { InputManager.Actions.Kitchen.OnSwipe += Add; }

        private List<InputManager.Swipe> currentCombo = new List<InputManager.Swipe>();

        private void OnComboFin(List<InputManager.Swipe> swipes) {
            Debug.Log($"KIYYYAA: {swipes[0]} {swipes[1]} {swipes[2]}");
            DishManager.instance.AddIngredient(swipes);
        }

        #endregion

        #region Public

        public void Add(InputManager.Swipe swipe) {
            if (skipNext) {
                skipNext = false;
                return;
            }
            currentCombo.Add(swipe);
            if (currentCombo.Count == 3) {
                OnComboFin(currentCombo);
                currentCombo.Clear();
            }
        }
        public void SkipNext() => skipNext = true;

        #endregion
    }
}