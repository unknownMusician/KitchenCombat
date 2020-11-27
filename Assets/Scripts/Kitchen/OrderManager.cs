using KC.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KC.Kitchen {
    public sealed class OrderManager : MonoBehaviour {

        #region Instance

        public static OrderManager instance { get; private set; }
        private void Awake() => instance = this;
        private void OnDestroy() => instance = null;

        #endregion

        #region Private

        #region Inspector

        [SerializeField] private Vector2 spawnPos = default;
        [SerializeField] private Vector2 endPos = default;
        [SerializeField] private float moveSpeed = default;
        [SerializeField] private float gap = default;

        #endregion

        private List<OrderHolder> orderHolders = new List<OrderHolder>();
        private Transform orderMenu = default;

        private void Start() => (orderMenu = new GameObject("Order menu").transform).SetParent(GameLogic.Kitchen.GameMenu);        

        private IEnumerator MoveOrder(OrderHolder holder) {
            var orderTransform = holder.order.transform;
            while (orderTransform.position.x > endPos.x + gap * holder.id) {
                orderTransform.position += (Vector3)Vector2.left * moveSpeed / 100;
                yield return new WaitForFixedUpdate();
            }
            orderTransform.position = endPos + Vector2.right * gap * holder.id;
            holder.coroutine = null;
        }

        private void OnDrawGizmos() {
            Vector2 size = Prefabs.Kitchen.order.GetComponent<BoxCollider2D>().size;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(spawnPos, size);
            Gizmos.color = new Color(1, 0.7f, 0);
            Gizmos.DrawWireCube(endPos, size);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(endPos + Vector2.right * gap, size);
        }

        #endregion

        #region Public

        public void AddOrder(Order order) {
            // todo: check if there is place
            // spawn
            var orderHolder = new OrderHolder(order, orderHolders.Count, endPos + Vector2.right * gap * orderHolders.Count);
            orderHolders.Add(orderHolder);
            order.transform.SetParent(orderMenu);
            order.transform.position = spawnPos;
            // start moving
            orderHolder.coroutineAuthor = this;
            orderHolder.coroutine = StartCoroutine(MoveOrder(orderHolder));
        }
        public bool RemoveOrder(OrderHolder orderHolder) {
            if (orderHolder == null) { return false; }
            if (orderHolder.coroutine != null) {
                orderHolder.coroutineAuthor.StopCoroutine(orderHolder.coroutine);
                orderHolder.coroutine = null;
            }
            bool result = orderHolders.Remove(orderHolder);
            // shift others
            foreach (var holder in orderHolders) {
                holder.id = orderHolders.IndexOf(holder);
                holder.regularPos = Vector2.right * gap * holder.id;
                holder.coroutineAuthor = this;
                holder.coroutine = StartCoroutine(MoveOrder(holder));
            }
            // 
            return result;
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
            public MonoBehaviour coroutineAuthor = default;
            public Vector2 regularPos = default;

            public OrderHolder(Order order, int id, Vector2 regularPos) {
                this.order = order;
                this.id = id;
                this.regularPos = regularPos;
            }
        }

        #endregion
    }
}