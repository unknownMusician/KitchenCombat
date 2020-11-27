using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {
    public Recipe Recipe { get; protected set; } = default;
    public Vector2 Size { get; protected set; } = default;
    public float Price { get; protected set; } = default;
    public float TimeToExpire { get; protected set; } = default;
    public int TableId { get; protected set; } = default;

    protected Order() { }
    public static Order Create(Recipe recipe, float timeToExpire, int tableId) {
        // Creating
        var order = Instantiate(GameLogic.Prefabs.Kitchen.order).GetComponent<Order>();
        order.Recipe = recipe;
        order.Price = recipe.Price;
        order.TimeToExpire = timeToExpire;
        order.TableId = tableId;
        var orderCollider = order.GetComponent<BoxCollider2D>();
        order.Size = orderCollider.size;
        Destroy(orderCollider);
        // Adding ingredients
        int size = recipe.Ingredients.Count;
        for (int i = 0; i < size; i++) {
            // Creating Order Note
            Transform noteTransform = new GameObject("OrderNote").transform;
            noteTransform.SetParent(order.transform);
            noteTransform.localPosition = new Vector2(-0.2f, 0.6f - 0.3f * i);
            // Filling Order Note; [0] - Icon; [1] - Text;
            var noteParts = new[] { new GameObject("NoteIcon", typeof(SpriteRenderer)), new GameObject("NoteText", typeof(SpriteRenderer)) };
            for(int j = 0; j < noteParts.Length; j++) {
                noteParts[j].GetComponent<SpriteRenderer>().sprite = j == 0 ? recipe.Ingredients[i].OrderIcon : recipe.Ingredients[i].OrderTextIcon;
                noteParts[j].GetComponent<SpriteRenderer>().sortingOrder = order.GetComponent<SpriteRenderer>().sortingOrder + 1; // todo: optimize
                noteParts[j].transform.SetParent(noteTransform);
                noteParts[j].transform.localPosition = j == 0 ? Vector2.left * 0.2f : Vector2.right * 0.4f;
            }
        }
        return order;
    }

    public OrderData TurnToData() {
        var orderData = new OrderData(this);
        Destroy(gameObject, 0.1f);
        return orderData;
    }

    public sealed class OrderData {

        // public Recipe Recipe { get; private set; } = default; // todo: remove
        public float Price { get; private set; } = default;
        public float TimeToExpire { get; private set; } = default;
        public int TableId { get; private set; } = default;
        public OrderData(Order order) {
            // this.Recipe = order.Recipe; // todo: remove
            this.Price = order.Price;
            this.TimeToExpire = order.TimeToExpire;
            this.TableId = order.TableId;
        }
    }
}
