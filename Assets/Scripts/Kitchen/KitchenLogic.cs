using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GameLogic;

/// todo:
/// - anim adding ingredient
/// 
public sealed class KitchenLogic : MonoBehaviour {

    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    [SerializeField] private Main main = default; // todo: unused
    [SerializeField] private ComboManager comboManager = default;
    [SerializeField] public Relations relations = default;
    [SerializeField] private OrderManager orderManager = default;
    [SerializeField] private DishManager dishManager = default;
    [SerializeField] private MovingManager movingManager = default;

    // todo: unused
    [System.Serializable]
    private sealed class Main {
        #region Constructor & k

        private KitchenLogic k;
        public void Start(KitchenLogic k) { this.k = k; }
        #endregion

    }
    [System.Serializable]
    private sealed class ComboManager {
        #region Constructor & k

        private KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
            InputManager.Actions.Kitchen.OnSwipe += Add;
        }
        #endregion

        private bool skipNext;

        private List<InputManager.Swipe> currentCombo = new List<InputManager.Swipe>();
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
        private void OnComboFin(List<InputManager.Swipe> swipes) {
            Debug.Log($"KIYYYAA: {swipes[0]} {swipes[1]} {swipes[2]}");
            k.dishManager.AddIngredient(swipes);
        }
        public void SkipNext() => skipNext = true;
    }
    [System.Serializable]
    public sealed class Relations {
        #region Constructor & k

        private KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
        }
        #endregion

        public bool ReceiveOrder(Order order) {
            k.orderManager.AddOrder(order);
            return true;
        }
    }
    [System.Serializable]
    private sealed class OrderManager {
        #region Constructor & k

        private KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
        }
        #endregion

        [SerializeField] private Transform orderMenu = default;
        [SerializeField] private GameObject orderPrefab = default;
        [SerializeField] private Vector2 spawnPos = default;
        [SerializeField] private Vector2 endPos = default;
        [SerializeField] private float moveSpeed = default;
        [SerializeField] private float gap = default;

        private List<OrderHolder> orderHolders = new List<OrderHolder>();

        public void AddOrder(Order order) {
            // todo: check if there is place
            // spawn
            var orderHolder = new OrderHolder(order, orderHolders.Count, endPos + Vector2.right * gap * orderHolders.Count);
            orderHolders.Add(orderHolder);
            order.transform.SetParent(orderMenu);
            order.transform.position = spawnPos;
            // start moving
            orderHolder.coroutine = k.StartCoroutine(MoveOrder(orderHolder));
        }
        public bool RemoveOrder(OrderHolder orderHolder) {
            if (orderHolder == null) { return false; }
            if (orderHolder.coroutine != null) {
                k.StopCoroutine(orderHolder.coroutine);
                orderHolder.coroutine = null;
            }
            bool result = orderHolders.Remove(orderHolder);
            // shift others
            foreach (var holder in orderHolders) {
                holder.id = orderHolders.IndexOf(holder);
                holder.regularPos = Vector2.right * gap * holder.id;
                holder.coroutine = k.StartCoroutine(MoveOrder(holder));
            }
            // 
            return result;
        }

        private IEnumerator MoveOrder(OrderHolder holder) {
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

        public OrderHolder GetOrder(Vector2 worldPosition) {
            foreach (var holder in orderHolders) {
                var order = holder.order;
                var orderPos = order.transform.position;
                var orderHalfSize = order.Size / 2.0f;
                if (orderPos.x + orderHalfSize.x > worldPosition.x &&
                    orderPos.x - orderHalfSize.x < worldPosition.x &&
                    orderPos.y + orderHalfSize.y > worldPosition.y &&
                    orderPos.y - orderHalfSize.y < worldPosition.y) {
                    return holder;
                }
            }
            return null;
        }

        public class OrderHolder {
            public Order order = default;
            public int id = default;
            public Coroutine coroutine = default;
            public Vector2 regularPos = default;

            public OrderHolder(Order order, int id, Vector2 regularPos) {
                this.order = order;
                this.id = id;
                this.regularPos = regularPos;
            }
        }
    }
    [System.Serializable]
    private sealed class DishManager {
        #region Constructor & k

        private KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
        }
        #endregion

        [SerializeField] private Vector2 dishPos = default;
        [SerializeField] private float gap = default;

        private DishBuilder actualDish = default;

        public void AddIngredient(List<InputManager.Swipe> swipes) {
            if (actualDish == null) {
                actualDish = DishBuilder.Create();
                actualDish.transform.position = dishPos;
            }

            var ingredient = Ingredient.Create(swipes);
            if (ingredient == null) { return; }

            actualDish.AddIngredient(ingredient, gap);
        }

        public bool CheckIfCorrect(Order order) {
            if (actualDish == null) { return false; }
            if (!Service.CompareLists(actualDish.ingredients, order.Recipe.Ingredients)) { return false; }
            return true;
        }

        public void OnDishFin(Order order) {
            // Creating instances for Restaurant
            var finDish = actualDish.Finalize();
            var orderData = order.TurnToData();
            // Fin corrections
            actualDish = null;
            // todo anim;
            k.StartCoroutine(DishFinalization(finDish.transform, 1,
                () => Restaurant.RecieveDish(finDish, orderData) // Send to Restaurant
                ));
        }

        private IEnumerator DishFinalization(Transform movable, float t, UnityAction end = null) {
            yield return k.StartCoroutine(Lerps.RotateLerp(movable, 180, t / 5.0f));
            yield return k.StartCoroutine(Lerps.RotateLerp(movable, 360, t / 5.0f));
            yield return new WaitForSeconds(2 * t / 5.0f);
            yield return k.StartCoroutine(Lerps.MoveLerp(movable, Vector2.right * Constants.SCREEN_WORLD_WIDTH, t / 5.0f));
            // todo
            end?.Invoke();
        }

        public void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(dishPos, Vector3.one);
        }
    }
    [System.Serializable]
    public sealed class MovingManager {
        #region Constructor & k

        private KitchenLogic k;
        public void Start(KitchenLogic k) {
            this.k = k;
            InputManager.Actions.Kitchen.OnPress += CheckOrderPress;
            InputManager.Actions.Kitchen.OnRelease += CheckOrderRelease;
        }
        #endregion

        private OrderManager.OrderHolder controlledOrderHolder = null;

        private void CheckOrderPress(Vector2 mouseWorldPosition) {
            var orderHolder = k.orderManager.GetOrder(mouseWorldPosition);
            if (orderHolder == null) { return; }

            k.comboManager.SkipNext();
            controlledOrderHolder = orderHolder;
            if (orderHolder.coroutine != null) {
                k.StopCoroutine(orderHolder.coroutine);
                orderHolder.coroutine = null;
            }
            orderHolder.coroutine = k.StartCoroutine(ControllOrder(orderHolder.order));
        }
        private void CheckOrderRelease(Vector2 mouseWorldPosition) {
            if (controlledOrderHolder != null) {
                k.StopCoroutine(controlledOrderHolder.coroutine);
                controlledOrderHolder.coroutine = null;
                // If dish created correctly
                var order = controlledOrderHolder.order;
                if (k.dishManager.CheckIfCorrect(order)) {
                    k.orderManager.RemoveOrder(controlledOrderHolder);
                    controlledOrderHolder = null;
                    k.dishManager.OnDishFin(order);
                    return;
                }
                // If not
                controlledOrderHolder.coroutine = k.StartCoroutine(
                    Lerps.MoveLerp(controlledOrderHolder.order.transform, controlledOrderHolder.regularPos, 0.2f, Lerps.Normalizators.Squared)
                    );
                controlledOrderHolder = null;
            }
        }

        private IEnumerator ControllOrder(Order order) {
            while (true) {
                yield return new WaitForFixedUpdate();
                order.transform.position = InputManager.MouseWorldPosition;
            }
        }
    }

    #region Mono

    private void Awake() {
        main.Start(this);
        comboManager.Start(this);
        relations.Start(this);
        orderManager.Start(this);
        dishManager.Start(this);
        movingManager.Start(this);
    }
    #endregion

    public void TEST() { // todo: TEST
        relations.ReceiveOrder(Order.Create(new Recipe(
            Prefabs.Kitchen.Ingredients.bread, Prefabs.Kitchen.Ingredients.meat, Prefabs.Kitchen.Ingredients.bread
            ), 10, 1));
    }

    private void OnDrawGizmos() {
        orderManager.OnDrawGizmos();
        dishManager.OnDrawGizmos();
    }
}