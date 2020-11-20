using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.ObjectModel;
using static GameLogic;


public class RestaurantLogic : MonoBehaviour 
{
    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    private Vector2 mouseDownPos;
    private float mouseDownTime;

    private Dish dish;

    public InspectorValues inspectorValues = new InspectorValues();
    [System.Serializable] public class InspectorValues 
    {
        [Header("Menus")]
        public Transform customersArrayMenu;
        public Transform tablesArrayMenu;
        public Transform restaurantMenu;
        public Transform dishesMenu;
        
        [Header("Points")]
        public Transform startPoint;
        public Transform exitPoint;
        public Transform waitPoint;
        public Transform dishPoint;
    }

    private void Awake()
    {
        inputManager = new InputManager(this);
    }

    private void Start() 
    {
        StartCoroutine(CustomerSpawning());
        dish = Instantiate(
            Prefabs.Restaurant.dish, 
            inspectorValues.dishPoint.position, 
            Quaternion.identity, 
            inspectorValues.dishesMenu
            ).GetComponent<Dish>();
    }

    private void OnEnable()
    {
        inputManager.OnSwipe += Throw;
        inputManager.OnTap += Drop;
    }

    private void OnDisable()
    {
        inputManager.OnSwipe -= Throw;
        inputManager.OnTap -= Drop;
        // todo
    }

    bool isThrown;
    void Throw(Vector2 dir) {
        if (isThrown) { return; }
        // todo 
        // dish.RigidbodyComponent.AddForce(dir);
        isThrown = true;
    }

    void Drop(Vector2 mouseWorldPos) {
        if(!isThrown) { return; }
        // todo
        // dish.RigidbodyComponent.velocity = Vector2.zero;
        // And so on...
        isThrown = false;
    }

    ///
    private void OnEnable() {
        InputManager.Actions.Restaurant.OnSwipe += Throw;
        InputManager.Actions.Restaurant.OnTap += Drop;
    }

    private void OnDisable() {
        InputManager.Actions.Restaurant.OnSwipe -= Throw; // Ctrl(Down) + K + C + Ctrl(Up) - comment
        InputManager.Actions.Restaurant.OnTap -= Drop; // // Ctrl(Down) + K + U + Ctrl(Up) - UNcomment
    }

    public void RecieveDish(Dish dish, Order.OrderData orderData) {
        print("Recieved");
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
        // todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo todo
    }

    private IEnumerator CustomerSpawning() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(CalculateDelay() + Random.Range(0, 5000) / 1000);
            SpawnCustomer();
        }
    }

    [SerializeField] private float delayCoefficient;
    private float CalculateDelay() 
    {
        // Метод расчитывает задержку между приходом покупателей
        // Единицы измерения результата - секунды

        // todo: Придумать формулу для рассчёта задержки (с учётом количества столов и количества рецептов/ингридиентов в наличии)

        int amountOfTables = inspectorValues.tablesArrayMenu.childCount;
        int amountOfRecipes = 3;

        float result = 1 / (delayCoefficient * (amountOfTables + amountOfRecipes));

        return result;
    }

    public void SpawnCustomer() 
    {
        Instantiate(Prefabs.Restaurant.customer,
            inspectorValues.startPoint.position,
            Quaternion.identity,
            inspectorValues.customersArrayMenu
            );
    }

    public bool AreThereFreeTables()
    {
        int tablesArrayLength = inspectorValues.tablesArrayMenu.childCount;

        for (int i = 0; i < tablesArrayLength; i++)
        {
            if (inspectorValues.tablesArrayMenu.GetChild(i).GetComponent<Table>().IsFree)
            {
                return true;
            }
        }

        return false;
    }

    public int GetRandomFreeTableNumber()
    {
        int tablesArrayLength = inspectorValues.tablesArrayMenu.childCount;
        Collection<int> freeTablesIndexes = new Collection<int>();

        for (int i = 0; i < tablesArrayLength; i++)
        {
            if (inspectorValues.tablesArrayMenu.GetChild(i).GetComponent<Table>().IsFree)
            {
                freeTablesIndexes.Add(i);
            }
        }

        if (freeTablesIndexes.Count != 0)
        {
            return freeTablesIndexes[Random.Range(0, freeTablesIndexes.Count)];
        }

        return -1;

    }

    bool isThrown;
    void Throw(Vector2 dir)
    {
        if (isThrown) { return; }
        dish.RigidbodyComponent.AddForce(dir);
        isThrown = true;
    }

    void Drop()
    {
        if (!isThrown) { return; }
        // dish.RigidbodyComponent.velocity = Vector2.zero;
        isThrown = false;
    }
}
