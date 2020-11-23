using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using static GameLogic;


public class RestaurantLogic : MonoBehaviour 
{
    #region Properties

    [SerializeField] private float delayCoefficient;
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
    public InspectorValues inspectorValues;

    public RectTransform UIMenu { get; set; } = default;
    public Transform GameMenu { get; set; } = default;

    private Vector2 mouseDownPos;
    private float mouseDownTime;
    private bool isThrown;
    private Dish dish;

    #endregion

    #region Behaviour methods

    private void Start() 
    {
        StartCoroutine(CustomerSpawning());
        dish = Instantiate(
            Prefabs.Restaurant.dish, 
            inspectorValues.dishPoint.position, 
            Quaternion.identity, 
            inspectorValues.dishesMenu
            ).GetComponent<Dish>();
        // UI.instance.OnViewChange += lambda or func_name (animate dish)
        // kitchen - true, restaurant - false
    }

    private void OnEnable()
    {
        GameLogic.InputManager.Actions.Restaurant.OnSwipe += Throw;
        GameLogic.InputManager.Actions.Restaurant.OnTap += Drop;
    }

    private void OnDisable()
    {
        GameLogic.InputManager.Actions.Restaurant.OnSwipe -= Throw;
        GameLogic.InputManager.Actions.Restaurant.OnTap -= Drop;
    }

    #endregion

    #region Methods

    public void RecieveDish(Dish dish, Order.OrderData orderData) {
        print("Recieved");
        // todo
    }

    private IEnumerator CustomerSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(CalculateDelay() + Random.Range(0, 5000) / 1000);
            SpawnCustomer();
        }
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

    private void SpawnCustomer() 
    {
        Instantiate(Prefabs.Restaurant.customer,
            inspectorValues.startPoint.position,
            Quaternion.identity,
            inspectorValues.customersArrayMenu
            );
    }

    private void Throw(Vector2 dir)
    {
        if (isThrown) { return; }
        dish.RigidbodyComponent.AddForce(dir);
        isThrown = true;
    }

    private void Drop(Vector2 dir)
    {
        if (!isThrown) { return; }
        dish.Land();
        // isThrown = false;
    }

    #endregion
}
