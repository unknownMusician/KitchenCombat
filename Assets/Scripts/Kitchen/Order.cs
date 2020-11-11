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
        var order = Instantiate(GameLogic.Prefabs.order).GetComponent<Order>();
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
            GameObject note = new GameObject("OrderNote");
            note.transform.SetParent(order.transform);
            note.transform.localPosition = new Vector2(-0.2f, 0.6f - 0.3f * i);
            // Filling Order Note
            GameObject noteIcon = new GameObject("NoteIcon", typeof(SpriteRenderer));
            GameObject noteText = new GameObject("NoteText", typeof(SpriteRenderer));
            noteIcon.GetComponent<SpriteRenderer>().sprite = recipe.Ingredients[i].OrderIcon;
            noteText.GetComponent<SpriteRenderer>().sprite = recipe.Ingredients[i].OrderTextIcon;
            noteIcon.GetComponent<SpriteRenderer>().sortingOrder = 22;
            noteText.GetComponent<SpriteRenderer>().sortingOrder = 22;
            noteIcon.transform.SetParent(note.transform);
            noteText.transform.SetParent(note.transform);
            noteIcon.transform.localPosition = Vector2.left * 0.2f;
            noteText.transform.localPosition = Vector2.right * 0.4f;
        }
        // Return
        return order;
        // todo: reduce
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
