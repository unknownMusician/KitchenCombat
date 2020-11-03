using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour 
{
    private RestaurantLogic restaurantLogic;
    private NavMeshAgent agentComponent;

    void Awake() 
    {
        restaurantLogic = GameLogic.Restaurant;
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
            Transform waitPoint = restaurantLogic.inspectorValues.waitPoint;
            agentComponent.SetDestination(waitPoint.position);
        }
    }

    public int CurrentTableNumber { get; set; }
    void GoToTable(int tableNumber) 
    {
        Transform currentTable = restaurantLogic.inspectorValues.tablesArrayMenu.GetChild(tableNumber);
        CurrentTableNumber = tableNumber;

        currentTable.GetComponent<Table>().IsFree = false;
        agentComponent.SetDestination(currentTable.position);
    }

    public void MakeAnOrder()
    {
        Debug.Log("Я сделал заказ!");
        // GameLogic.Kitchen.restaurant.ReceiveOrder(new Order(...)) // todo: связь
    }

    public void LeaveRestaurant()
    {
        agentComponent.SetDestination(restaurantLogic.inspectorValues.exitPoint.position);
        Table usedTable = restaurantLogic.inspectorValues.tablesArrayMenu.GetChild(CurrentTableNumber).GetComponent<Table>();
        usedTable.IsFree = true;
        CurrentTableNumber = -1;
    }
}
