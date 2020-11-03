using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenLogic : MonoBehaviour {

    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    [SerializeField] protected Main main = default; // todo: unused
    [SerializeField] protected ComboManager combo = default;
    [SerializeField] public Relations relations = default;
    [SerializeField] protected OrderManager orderManager = default;
    [SerializeField] protected DishCreator dishCreator = default;

    // todo: unused
    [System.Serializable] public class Main {
        #region Constructor & k

        protected KitchenLogic k;
        public void Start(KitchenLogic k) {this.k = k;}
        #endregion

    }
    [System.Serializable] public class ComboManager {
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
            k.dishCreator.AddIngredient(swipes);
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

        [SerializeField] protected Transform orderMenu = default;
        [SerializeField] protected GameObject orderPrefab = default;
        [SerializeField] protected Vector2 spawnPos = default;
        [SerializeField] protected Vector2 endPos = default;
        [SerializeField] protected float moveSpeed = default;
        [SerializeField] protected float gap = default;

        protected List<OrderHolder> orderHolders = new List<OrderHolder>();

        public void AddOrder(Order order) {
            // todo: check if there is place
            // spawn
            var orderHolder = new OrderHolder(order, orderHolders.Count);
            orderHolders.Add(orderHolder);
            order.transform.SetParent(orderMenu);
            order.transform.position = spawnPos;
            // start moving
            orderHolder.coroutine = k.StartCoroutine(MoveOrder(orderHolder));
            // todo: spawn
        }

        protected IEnumerator MoveOrder(OrderHolder holder) {
            var orderTransform = holder.order.transform;
            while (orderTransform.position.x > endPos.x + gap * holder.id) {
                orderTransform.position += (Vector3)Vector2.left * moveSpeed / 100;
                yield return new WaitForFixedUpdate();
            }
            orderTransform.position = endPos + Vector2.right * gap * holder.id;
            holder.coroutine = null;
        }

        public void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(spawnPos, orderPrefab.GetComponent<BoxCollider2D>().size);
            Gizmos.color = new Color(1, 0.7f, 0);
            Gizmos.DrawWireCube(endPos, orderPrefab.GetComponent<BoxCollider2D>().size);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(endPos + Vector2.right * gap, orderPrefab.GetComponent<BoxCollider2D>().size);
        }

        public class OrderHolder {
            public Order order = default;
            public int id = default;
            public Coroutine coroutine = default;

            public OrderHolder(Order order, int id) {
                this.order = order;
                this.id = id;
            }
        }
    }
    [System.Serializable] public class DishCreator {
        #region Constructor & k

        protected KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
        }
        #endregion

        [SerializeField] protected Vector2 dishPos = default;
        [SerializeField] protected float gap = default;

        protected Dish actualDish = default;

        public void AddIngredient(List<GameLogic.SwipeType> swipes) {
            if(actualDish == null) {
                actualDish = Dish.Create();
                actualDish.transform.position = dishPos;
            }

            var ingredient = Ingredient.GetIngredient(swipes);
            if(ingredient == null) { return; }

            actualDish.AddIngredient(ingredient, gap);
            // todo: spawn
        }

        public void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(dishPos, Vector3.one);
            // todo: Gizmos
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
        relations.ReceiveOrder(Order.Create(Recipe.GenerateRecipe(), 1.00f, 10, 1));
    }

    protected void OnDrawGizmos() {
        orderManager.OnDrawGizmos();
        dishCreator.OnDrawGizmos();
    }
}