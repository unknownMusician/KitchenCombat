using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RestaurantLogic : MonoBehaviour 
{
    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    public Transform restaurantMenu;

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
            restaurantMenu.GetChild(3).GetChild(0).position,
            Quaternion.identity,
            restaurantMenu.GetChild(1)
            );
    }
}
