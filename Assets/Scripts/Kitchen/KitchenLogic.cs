using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KitchenLogic : MonoBehaviour {

    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    [SerializeField] protected Main main = default; // todo: unused
    [SerializeField] protected InputManager inputManager = default;
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
        public void Start(KitchenLogic k) {
            this.k = k;
            k.inputManager.OnSwipe += Add;
        }
        #endregion

        protected List<Swipe> currentCombo = new List<Swipe>();
        public void Add(Swipe swipe) {
            currentCombo.Add(swipe);
            if (currentCombo.Count == 3) {
                OnComboFin(currentCombo);
                currentCombo.Clear();
            }
        }
        protected void OnComboFin(List<Swipe> swipes) {
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
        // todo
        public bool CheckForOrder(Vector2 worldPosition) {
            Return true;
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

        protected DishBuilder actualDish = default;

        public void AddIngredient(List<Swipe> swipes) {
            if(actualDish == null) {
                actualDish = DishBuilder.Create();
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
        }
    }
    [System.Serializable] public class InputManager {
        #region Constructor & k

        protected KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
            UI.instance.OnViewChange += CheckView;
            coroutine = k.StartCoroutine(WaitForPress());
        }
        #endregion

        public UnityAction<Swipe> OnSwipe = default;
        public UnityAction OnTap = default;

        protected Coroutine coroutine = default;
        protected Vector2 mouseDownPos = default;
        protected float mouseDownTime = default;

        public void CheckView(bool isKitchen) {
            if (isKitchen) {
                if (coroutine == null) { coroutine = k.StartCoroutine(WaitForPress()); }
            } else {
                if (coroutine != null) {
                    k.StopCoroutine(coroutine);
                    coroutine = null;
                }
            }
        }
        public IEnumerator WaitForPress() {
            while (true) {
                if (Input.GetMouseButtonDown(0)) {
                    mouseDownPos = Input.mousePosition;
                    mouseDownTime = Time.time;
                    coroutine = k.StartCoroutine(WaitForRelease());
                    break;
                }
                yield return null;
            }
        }
        public IEnumerator WaitForRelease() {
            while (true) {
                if (Input.GetMouseButtonUp(0)) {
                    var localDir = (Vector2)Input.mousePosition - mouseDownPos;
                    if (Time.time - mouseDownTime < 0.05f || localDir.magnitude < 30) {
                        OnTap?.Invoke();
                    } else {
                        DoSwipe(localDir);
                    }
                    coroutine = k.StartCoroutine(WaitForPress());
                    break;
                }
                yield return null;
            }
        }

        protected void DoSwipe(Vector2 direction) { // todo: make swipes only work when in Kitchen
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                if (direction.x == 0)
                    return;
                else if (direction.x > 0)
                    OnSwipe?.Invoke(Swipe.Right);
                else
                    OnSwipe?.Invoke(Swipe.Left);
            } else {
                if (direction.y == 0)
                    return;
                else if (direction.y > 0)
                    OnSwipe?.Invoke(Swipe.Up);
                else
                    OnSwipe?.Invoke(Swipe.Down);
            }
        }
    }
    public enum Swipe { Up, Down, Left, Right }

    #region Mono

    protected void Awake() {
        main.Start(this);
        inputManager.Start(this);
        combo.Start(this);
        relations.Start(this);
        orderManager.Start(this);
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
