using UnityEngine;
using System.Threading;

public class Table : MonoBehaviour 
{
    public bool IsFree { get; set; }
    private Transform transformComponent;
    private RestaurantLogic restaurant;
    private int OrderInTablesArray { get; set; }

    void Awake()
    {
        IsFree = true;
        transformComponent = GetComponent<Transform>();
    }

    void Start()
    {
        restaurant = GameLogic.Restaurant;
        Transform tablesArray = restaurant.TablesArray;

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

        if ((IsFree == true) & (OrderInTablesArray == otherCustomerComponent.CurrentTableNumber)) 
        {
            /* otherCustomerComponent.MakeAnOrder();
            otherCustomerComponent.LeaveRestaurant(); */
        }
    }
}
