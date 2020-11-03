using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenLogic : MonoBehaviour {

    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    public Main main = default; // todo: unused
    public Combo combo = default;
    public Relations relations = default;
    public OrderManager orderManager = default;

    // todo: unused
    [System.Serializable] public class Main {
        #region Constructor & k

        protected KitchenLogic k;
        public void Start(KitchenLogic k) {this.k = k;}
        #endregion

    }
    [System.Serializable] public class Combo {
        #region Constructor & k

        protected KitchenLogic k;
        public void Start(KitchenLogic k) { this.k = k; }
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
    [System.Serializable] public class Relations {
        #region Constructor & k

        protected KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
        }
        #endregion

        public bool ReceiveOrder(Order order) {
            k.orderManager.AddOrder(order);
            return true;
        }
    }
    [System.Serializable] public class OrderManager {
        #region Constructor & k

        protected KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
        }
        #endregion

        [SerializeField] private Transform orderMenu = default;
        [SerializeField] private GameObject orderPrefab = default;
        [SerializeField] private Vector2 spawnPos = default;
        [SerializeField] private float moveSpeed = default;

        private List<OrderHolder> orderHolders = new List<OrderHolder>();

        public void AddOrder(Order order) {
            // todo: check if there is place
            // spawn
            orderHolders.Add(new OrderHolder(order, orderHolders.Count));
            Instantiate(order.gameObject, spawnPos, Quaternion.identity, orderMenu);
            // todo: spawn
        }

        private IEnumerator MoveOrders() {
            while (true) {
                foreach (var orderHolder in orderHolders) {
                    orderHolder.order.transform.position += (Vector3)Vector2.left * -moveSpeed;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(spawnPos, orderPrefab.GetComponent<BoxCollider2D>().size);
        }

        public class OrderHolder {
            public Order order;
            public int id;

            public OrderHolder(Order order, int id) {
                this.order = order;
                this.id = id;
            }
        }
    }

    #region Mono

    protected void Awake() {
        main.Start(this);
        combo.Start(this);
        relations.Start(this);
        orderManager.Start(this);
    }
    protected void OnEnable() {
        GameLogic.L.OnSwipe += combo.Add;
    }
    #endregion

    public void TEST() { // todo: TEST
        orderManager.AddOrder(new Order(Order.Recipe.GenerateRecipe(), 10, 1));
    }

    private void OnDrawGizmos() {
        orderManager.OnDrawGizmos();
    }
}