using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour 
{
    private RestaurantLogic restaurantLogic;
    private Transform restaurantMenu;
    private NavMeshAgent agentComponent;

    void Awake() 
    {
        restaurantLogic = GameLogic.Restaurant;
        restaurantMenu = restaurantLogic.restaurantMenu;
        agentComponent = GetComponent<NavMeshAgent>();
    }

    void Start() 
    {
        agentComponent.updateRotation = false;
        agentComponent.updateUpAxis = false;

        if (restaurantLogic.AreThereFreeTables())
        {
            GoToTable(restaurantLogic.GetRandomFreeTableNumber());
        }
        else
        {
            Transform waitPoint = restaurantMenu.GetChild(2).GetChild(2);
            agentComponent.SetDestination(waitPoint.position);
        }
    }

    public int CurrentTableNumber { get; set; }
    void GoToTable(int tableNumber) 
    {
        Transform currentTable = restaurantMenu.GetChild(1).GetChild(1).GetChild(tableNumber);
        CurrentTableNumber = tableNumber;

        currentTable.GetComponent<Table>().IsFree = false;
        agentComponent.SetDestination(currentTable.position);
    }

    public void MakeAnOrder()
    {
        Debug.Log("Я сделал заказ!");
    }

    public void LeaveRestaurant()
    {
        agentComponent.SetDestination(restaurantMenu.GetChild(2).GetChild(1).position);
        restaurantMenu.GetChild(1).GetChild(1).GetChild(CurrentTableNumber).GetComponent<Table>().IsFree = true;
        CurrentTableNumber = -1;
    }
}
