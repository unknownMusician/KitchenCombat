using System.Collections;
using UnityEngine;

public class RestaurantLogic : MonoBehaviour 
{
    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;
    public Transform restaurantMenu;
    public Transform TablesArray { get; set; }

    [SerializeField] private InspectorValues inspectorValues = new InspectorValues();
    [System.Serializable] public class InspectorValues {
        public Transform TablesArray;
        public Transform menu123;
        public GameObject prefab1;
        public GameObject prefab2;
        public GameObject prefab3;
    }

    void Awake()
    {
        TablesArray = restaurantMenu.GetChild(1).GetChild(1);
    }

    void Start() 
    {
        StartCoroutine(CustomerSpawning());
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

        int amountOfTables = TablesArray.childCount;
        int amountOfRecipes = 3;

        float result = 1 / (delayCoefficient * (amountOfTables + amountOfRecipes));

        return result;
    }

    [SerializeField] private GameObject customerPrefab;
    public void SpawnCustomer() 
    {
        Instantiate(customerPrefab,
            restaurantMenu.GetChild(2).GetChild(0).position,
            Quaternion.identity,
            restaurantMenu.GetChild(0)
            );
    }

    public bool AreThereFreeTables()
    {
        Transform tablesArrayMenu = restaurantMenu.GetChild(1).GetChild(1);
        int tablesArrayLength = restaurantMenu.GetChild(1).GetChild(1).childCount;

        for (int i = 0; i < tablesArrayLength; i++)
        {
            if (tablesArrayMenu.GetChild(i).GetComponent<Table>().IsFree)
            {
                return true;
            }
        }

        return false;
    }

    public int GetRandomFreeTableNumber()
    {
        Transform tablesArrayMenu = restaurantMenu.GetChild(1).GetChild(1);
        int tablesArrayLength = restaurantMenu.GetChild(1).GetChild(1).childCount;

        int randomNumber = Random.Range(0, tablesArrayLength);
        Table randomTable = tablesArrayMenu.GetChild(randomNumber).GetComponent<Table>();

        while(randomTable.IsFree == false)
        {
            randomNumber = Random.Range(0, tablesArrayLength);
            randomTable = tablesArrayMenu.GetChild(randomNumber).GetComponent<Table>();
        }

        return randomNumber;
    }
}
