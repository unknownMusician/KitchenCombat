using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenLogic : MonoBehaviour {

    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    public Combo combo = default;
    public Relations restaurant = default;
    public class Combo {
        #region Constructor & k

        protected KitchenLogic k;
        public Combo(KitchenLogic k) {
            this.k = k;
        }
        #endregion

        protected List<GameLogic.SwipeType> currentCombo = new List<GameLogic.SwipeType>();
        public void Add(GameLogic.SwipeType swipe) {
            currentCombo.Add(swipe);
            if (currentCombo.Count == 3) {
                OnComboFin(currentCombo);
                currentCombo.Clear();
            }
        }
        protected void OnComboFin(List<GameLogic.SwipeType> swipes) {
            Debug.Log($"KIYYYAA: {swipes[0]} {swipes[1]} {swipes[2]}");
            // todo: spawn
        }
    }

    public class Relations {
        public bool ReceiveOrder(Order order) {

            // todo
            return true;
        }
    }

    #region Mono

    protected void Awake() {
        combo = new Combo(this);
    }
    protected void OnEnable() {
        GameLogic.L.OnSwipe += combo.Add;
    }
    #endregion
}