using System.Collections;
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
            int amountOfCustomers = restaurantLogic.inspectorValues.customersArrayMenu.childCount;
            int amountOfTables = restaurantLogic.inspectorValues.tablesArrayMenu.childCount;

            Transform waitPoint = restaurantLogic.inspectorValues.waitPoint;
            agentComponent.SetDestination(new Vector2(waitPoint.position.x + (amountOfCustomers - amountOfTables - 1) * 0.8f, waitPoint.position.y));
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

    public IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.75f);
        agentComponent.stoppingDistance = 0.25f;
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
