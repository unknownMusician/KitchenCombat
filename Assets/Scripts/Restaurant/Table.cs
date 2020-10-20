using UnityEngine;
using System.Threading;

public class Table : MonoBehaviour 
{
    public bool IsFree { get; set; }
    [SerializeField] private int orderInTablesArray;

    void Awake()
    {
        IsFree = true;
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Customer otherCustomerComponent = other.collider.GetComponent<Customer>();

        if ((IsFree == true) & (orderInTablesArray == otherCustomerComponent.currentTableNumber)) 
        {
            IsFree = false;
            otherCustomerComponent.MakeAnOrder();
            otherCustomerComponent.LeaveRestaurant();
        }
    }
}
