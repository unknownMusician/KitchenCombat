using System.Collections;
using UnityEngine;

public class RestaurantLogic : MonoBehaviour 
{
    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    public Transform restaurantMenu;
    public GameObject navMesh;

    void Start() 
    {
        /* Instantiate(customerPrefab,
            restaurantMenu.GetChild(2).GetChild(0).position,
            Quaternion.identity,
            restaurantMenu.GetChild(0)
            ); */
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

    [SerializeField] private Transform tablesArray;
    [SerializeField] float delayCoefficient;

    private float CalculateDelay() 
    {
        // Метод расчитывает задержку между приходом покупателей
        // Единицы измерения результата - секунды

        // todo: Придумать формулу для рассчёта задержки (с учётом количества столов и количества рецептов/ингридиентов в наличии)

        int amountOfTables = tablesArray.childCount;
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
