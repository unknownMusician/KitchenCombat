using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameLogic;

/// todo:
/// - Check order by position of click +
/// - Move Order with cursor +
/// - Disable swipes temporarely when controlling +
/// - Make Ingredient images in order +
/// - Mouse release check (compare to order)
/// - if not same - anim back down
/// - if same - generate dish and remove order from list
/// - send to Restaurant
/// - and anim "creating dish"
/// 
/// 
public sealed class KitchenLogic : MonoBehaviour {

    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    [SerializeField] private Main main = default; // todo: unused
    [SerializeField] private ComboManager comboManager = default;
    [SerializeField] public Relations relations = default;
    [SerializeField] private OrderManager orderManager = default;
    [SerializeField] private DishCreator dishCreator = default;
    [SerializeField] private MovingManager movingManager = default;

    // todo: unused
    [System.Serializable]
    public sealed class Main {
        #region Constructor & k

        private KitchenLogic k;
        public void Start(KitchenLogic k) { this.k = k; }
        #endregion

    }
    [System.Serializable]
    public sealed class ComboManager {
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
            k.dishCreator.AddIngredient(swipes);
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
    public sealed class OrderManager {
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
            // todo: spawn
        }
        public bool RemoveOrder(OrderHolder orderHolder) {
            if (orderHolder == null) { return false; }
            if (orderHolder.coroutine != null) {
                k.StopCoroutine(orderHolder.coroutine);
                orderHolder.coroutine = null;
            }
            return orderHolders.Remove(orderHolder);
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
    public sealed class DishCreator {
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

            var ingredient = Ingredient.GetIngredient(swipes);
            if (ingredient == null) { return; }

            actualDish.AddIngredient(ingredient, gap);
            // todo: spawn
        }

        public bool CheckIfCorrect(Order order) {
            List<Ingredient> list1 = actualDish.ingredients;
            List<Ingredient> list2 = order.Recipe.Ingredients;
            int size = list1.Count;
            if (list1.Count != list2.Count) { return false; }
            for (int i = 0; i < size; i++) {
                if (list1[i] != list2[i]) { return false; }
            }
            // Creating instances for Restaurant
            var finDish = actualDish.Finalize();
            var orderData = order.TurnToData();
            // Fin corrections
            Destroy(actualDish.gameObject);
            actualDish = null;
            // Send to Restaurant
            Restaurant.RecieveDish(finDish, orderData);
            // todo anim;
            return true;
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
            InputManager.Actions.Kitchen.OnPress += CheckOrderClick;
            InputManager.Actions.Kitchen.OnRelease += CheckOrderRelease;
        }
        #endregion

        private OrderManager.OrderHolder controlledOrderHolder = null;

        private void CheckOrderClick(Vector2 mouseWorldPosition) {
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
                // if dish created correctly
                if (k.dishCreator.CheckIfCorrect(controlledOrderHolder.order)) {
                    k.orderManager.RemoveOrder(controlledOrderHolder);
                    controlledOrderHolder = null;
                    return;
                }
                controlledOrderHolder.order.transform.position = controlledOrderHolder.regularPos;
                // todo: or start the animation instead
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
        movingManager.Start(this);
    }
    #endregion

    public void TEST() { // todo: TEST
        relations.ReceiveOrder(Order.Create(new Recipe(Ingredient.Ingredients.Bread, Ingredient.Ingredients.Meat, Ingredient.Ingredients.Bread), 10, 1));
    }

    private void OnDrawGizmos() {
        orderManager.OnDrawGizmos();
        dishCreator.OnDrawGizmos();
    }
}