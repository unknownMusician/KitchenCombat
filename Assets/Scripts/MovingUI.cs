using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUI : MonoBehaviour {

    public static MovingUI instance;

    [SerializeField] protected RectTransform RestaurantUI = null;
    [SerializeField] protected RectTransform KitchenUI = null;

    protected bool kitchenLook = true;

    protected Coroutine movingCoroutine = null;

    protected void Awake() {
        if (instance != null) { instance = this; }
    }

    protected void Start() {
        // set size
        var size = GetComponent<RectTransform>().sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta;
        RestaurantUI.GetComponent<RectTransform>().sizeDelta = size;
        KitchenUI.GetComponent<RectTransform>().sizeDelta = size;
        //set pos
        RestaurantUI.localPosition = Vector3.right * Camera.main.pixelWidth;
    }

    public void BtnClickRestaurant() {
        if(movingCoroutine != null) {
            StopCoroutine(movingCoroutine);
        }
        movingCoroutine = StartCoroutine(Move());
    }

    protected IEnumerator Move() {
        kitchenLook = !kitchenLook;
        // float cameraWidth = Camera.main.pixelWidth;
        float cameraWidthWorld = Mathf.Abs(Camera.main.ScreenToWorldPoint(Vector3.left).x) * 2;
        var camTransform = Camera.main.transform;
        float t = 0.4f;
        float l = 0;
        while (l < 1) {
            float newL = LerpNormalize1(l);
            float lerp = kitchenLook ? Mathf.Lerp(cameraWidthWorld, 0, newL) : Mathf.Lerp(0, cameraWidthWorld, newL);
            // transform.position = Vector3.left * Mathf.Lerp(0, cameraWidth, lerp); // move UI
            camTransform.position = new Vector3(lerp, camTransform.position.y, camTransform.position.z); // move camera
            l += 1 / t * Time.deltaTime;
            yield return null; // todo
        }
        var fin = kitchenLook ? 0 : cameraWidthWorld;
        // transform.position = Vector3.left * cameraWidth; // move UI
        camTransform.position = new Vector3(fin, camTransform.position.y, camTransform.position.z); // move camera
    }

    protected float LerpNormalize1(float l) {
        float x = (-1 + Mathf.Sqrt(5)) / 2;
        return -1 / (l + x) + x + 1;
    }
}
