﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using KC.Kitchen;

namespace KC.Common {
    public sealed class UI : MonoBehaviour {

        public static UI instance;

        public Common common = null;
        public Restaurant restaurant = null;
        public Kitchen kitchen = null;

        public UnityAction<bool> OnViewChange { get; set; } = default;

        private void Awake() {
            if (instance != null) { return; }
            instance = this;

            common.Start(this);
            restaurant.Start(this);
            kitchen.Start(this);
        }

        public void BtnClickRestaurantKitchen() => common.BtnClickRestaurantKitchen();

        [System.Serializable]
        public class Common {
            protected UI ui = null;

            protected bool _kitchenLook = true;
            public bool KitchenLook {
                get => _kitchenLook;
                private set {
                    _kitchenLook = value;
                    ui.OnViewChange?.Invoke(_kitchenLook);
                }
            }

            protected Coroutine movingCoroutine = null;

            [SerializeField] protected RectTransform CommonUI = null;
            [SerializeField] protected Button restaurantKitchenBtn;

            public void Start(UI ui) {
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
                movingCoroutine = ui.StartCoroutine(Move(ui));
                // change text
                restaurantKitchenBtn.GetComponentInChildren<TextMeshProUGUI>().text = KitchenLook ? "Kitchen" : "Restaurant";
            }
            protected IEnumerator Move(UI ui) {
                KitchenLook = !KitchenLook;
                float cameraWidthPixels = Camera.main.pixelWidth;
                float cameraWidthWorld = Constants.SCREEN_WORLD_WIDTH;
                var camTransform = Camera.main.transform;
                float t = 0.2f;
                float l = 0;
                while (l < 1) {
                    float newL = Lerps.Normalizators.Squared(l);
                    float lerp = KitchenLook ? Mathf.Lerp(1, 0, newL) : Mathf.Lerp(0, 1, newL);
                    camTransform.position = new Vector3(lerp * cameraWidthWorld, camTransform.position.y, camTransform.position.z); // move camera
                    ui.transform.position = Vector2.zero; // move UI
                    l += 1 / t * Time.deltaTime;
                    yield return null; // todo
                }
                var fin = KitchenLook ? 0 : cameraWidthWorld;
                camTransform.position = new Vector3(fin, camTransform.position.y, camTransform.position.z); // move camera
                ui.transform.position = Vector2.zero; // move UI
            }
        }
        [System.Serializable]
        public class Restaurant {
            protected UI ui = null;

            [SerializeField] protected RectTransform RestaurantUI = null;

            public void Start(UI ui) {
                this.ui = ui;
                // set Pos
                RestaurantUI.localPosition = Vector3.right * Camera.main.pixelWidth;
            }

            public void SetSize(Vector2 size) {
                RestaurantUI.GetComponent<RectTransform>().sizeDelta = size;
            }
        }
        [System.Serializable]
        public class Kitchen {
            protected UI ui = null;

            [SerializeField] protected RectTransform KitchenUI = null;

            public void Start(UI ui) {
                this.ui = ui;
            }

            public void SetSize(Vector2 size) {
                KitchenUI.GetComponent<RectTransform>().sizeDelta = size;
            }
        }
    }
}