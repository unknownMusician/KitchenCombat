using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour 
{
    private RestaurantLogic restaurantLogic;
    private Transform restaurantMenu;
    private NavMeshAgent agentComponent;
    private BoxCollider2D boxColliderComponent;

    void Awake() 
    {
        restaurantLogic = GameLogic.Restaurant;
        restaurantMenu = restaurantLogic.restaurantMenu;
        agentComponent = GetComponent<NavMeshAgent>();
        boxColliderComponent = GetComponent<BoxCollider2D>();
    }

    void Start() 
    {
        agentComponent.updateRotation = false;
        agentComponent.updateUpAxis = false;

        GoToTable(restaurantLogic.GetRandomFreeTableNumber());
    }

    public int currentTableNumber;

    void GoToTable(int tableNumber) 
    {
        agentComponent.SetDestination(restaurantMenu.GetChild(1).GetChild(1).GetChild(tableNumber).position);
        currentTableNumber = tableNumber;
    }

    public void MakeAnOrder()
    {
        
    }

    public void LeaveRestaurant()
    {
        agentComponent.SetDestination(restaurantMenu.GetChild(2).GetChild(1).position);
        restaurantMenu.GetChild(1).GetChild(1).GetChild(currentTableNumber).GetComponent<Table>().IsFree = true;
        currentTableNumber = -1;
    }
}
