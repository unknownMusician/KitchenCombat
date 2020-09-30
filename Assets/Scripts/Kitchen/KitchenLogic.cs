using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenLogic : MonoBehaviour {

    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    protected List<GameLogic.SwipeType> currentCombo = new List<GameLogic.SwipeType>();

    protected void OnComboFin() { Debug.Log($"KIYYYAA: {currentCombo[0]} {currentCombo[1]} {currentCombo[2]}"); }

    private void Awake() {

        GameLogic.L.OnSwipe += (dir) => {
            currentCombo.Add(dir);
            Debug.Log(dir);
            if (currentCombo.Count == 3) {
                OnComboFin();
                currentCombo.Clear();
            }
        };
    }

}