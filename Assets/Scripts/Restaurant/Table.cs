using UnityEngine;
using UnityEngine.AI;

public class Table : MonoBehaviour 
{
    private RestaurantLogic restaurant;
    private Transform transformComponent;

    public bool IsFree { get; set; }
    private int OrderInTablesArray { get; set; }

    void Awake()
    {
        IsFree = true;
        transformComponent = GetComponent<Transform>();
    }

    void Start()
    {
        restaurant = GameLogic.Restaurant;
        Transform tablesArray = restaurant.inspectorValues.tablesArrayMenu;

        for (int i = 0; i < tablesArray.childCount; i++)
        {
            if (tablesArray.GetChild(i).name == transformComponent.name)
            {
                OrderInTablesArray = i;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Customer otherCustomerComponent = other.collider.GetComponent<Customer>();

        if (OrderInTablesArray == otherCustomerComponent.CurrentTableNumber) 
        {
            otherCustomerComponent.StartCoroutine("Stop");
            /* otherCustomerComponent.MakeAnOrder();
            otherCustomerComponent.LeaveRestaurant(); */
        }
    }
}
