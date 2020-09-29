using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenLogic : MonoBehaviour {

    protected Vector2 mouseDownPos;
    protected float mouseDownTime;

    protected List<SwipeType> currentCombo = new List<SwipeType>();

    protected void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mouseDownPos = Input.mousePosition;
            mouseDownTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0)) {
            var localDir = (Vector2)Input.mousePosition - mouseDownPos;
            if (Time.time - mouseDownTime < 0.05f || localDir.magnitude < 30)
                Tap();
            else
                Swipe(localDir);
        }
    }
    protected void Swipe(Vector2 direction) {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x == 0)
                return;
            else if (direction.x > 0)
                Swipe(SwipeType.Right);
            else
                Swipe(SwipeType.Left);
        } else {
            if (direction.y == 0)
                return;
            else if (direction.y > 0)
                Swipe(SwipeType.Up);
            else
                Swipe(SwipeType.Down);
        }
    }
    protected void Swipe(SwipeType dir) {
        currentCombo.Add(dir);
        Debug.Log(dir);
        if (currentCombo.Count == 3) {
            OnComboFin();
            currentCombo.Clear();
        }
    }
    protected void Tap() { Debug.Log("tap"); }
    protected void OnComboFin() { Debug.Log($"KIYYYAA: {currentCombo[0]} {currentCombo[1]} {currentCombo[2]}"); }

    public enum SwipeType { Up, Down, Left, Right }
}