using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovingUI : MonoBehaviour {

    public static MovingUI instance;

    public Common common = null;
    public Restaurant restaurant = null;
    public Kitchen kitchen = null;


    protected void Awake() {
        if (instance != null) { return; }
        instance = this;

        common.Start(this);
        restaurant.Start(this);
        kitchen.Start(this);
    }

    public void BtnClickRestaurantKitchen() => common.BtnClickRestaurantKitchen();

    [System.Serializable] public class Common {
        protected MovingUI ui = null;

        protected bool kitchenLook = true;

        protected Coroutine movingCoroutine = null;

        [SerializeField] protected RectTransform CommonUI = null;
        [SerializeField] protected Button restaurantKitchenBtn;

        public void Start(MovingUI ui) {
            this.ui = ui;
            // set size
            var size = ui.GetComponent<RectTransform>().sizeDelta = ui.transform.parent.GetComponent<RectTransform>().sizeDelta;
            SetSize(size);
            ui.restaurant.SetSize(size);
            ui.kitchen.SetSize(size);
        }

        public void SetSize(Vector2 size) {
            CommonUI.GetComponent<RectTransform>().sizeDelta = size;
        }

        public void BtnClickRestaurantKitchen() {
            if (movingCoroutine != null) {
                ui.StopCoroutine(movingCoroutine);
                movingCoroutine = null;
            }
            movingCoroutine = ui.StartCoroutine(Move());
            // change text
            restaurantKitchenBtn.GetComponentInChildren<TextMeshProUGUI>().text = kitchenLook ? "Kitchen" : "Restaurant";
        }
        protected IEnumerator Move() {
            kitchenLook = !kitchenLook;
            // float cameraWidth = Camera.main.pixelWidth;
            float cameraWidthWorld = Mathf.Abs(Camera.main.ScreenToWorldPoint(Vector3.left).x) * 2;
            var camTransform = Camera.main.transform;
            float t = 0.2f;
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
    [System.Serializable] public class Restaurant {
        protected MovingUI ui = null;

        [SerializeField] protected RectTransform RestaurantUI = null;

        public void Start(MovingUI ui) {
            this.ui = ui;
            // set Pos
            RestaurantUI.localPosition = Vector3.right * Camera.main.pixelWidth;
        }

        public void SetSize(Vector2 size) {
            RestaurantUI.GetComponent<RectTransform>().sizeDelta = size;
        }
    }
    [System.Serializable] public class Kitchen {
        protected MovingUI ui = null;

        [SerializeField] protected RectTransform KitchenUI = null;

        public void Start(MovingUI ui) {
            this.ui = ui;
        }

        public void SetSize(Vector2 size) {
            KitchenUI.GetComponent<RectTransform>().sizeDelta = size;
        }
    }
}
