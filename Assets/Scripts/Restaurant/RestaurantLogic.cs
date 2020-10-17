using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class RestaurantLogic : MonoBehaviour {

    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    private CustomerLogic customerLogic;

    void Awake() {
        GameMenu = GetComponent<Transform>();
        customerLogic = GetComponent<CustomerLogic>();
    }

    void Start() {
        StartCoroutine(CustomerSpawning());
    }

    private IEnumerator CustomerSpawning() {
        while (true) {
            yield return new WaitForSeconds(CalculateDelay() + Random.Range(0, 5000) / 1000);
            customerLogic.SpawnCustomer();
        }
    }

    [SerializeField] private Transform tablesArray;
    [SerializeField] float coefficient;

    private float CalculateDelay() {

        // Метод расчитывает задержку между приходом покупателей
        // Единицы измерения результата - секунды

        // todo: Придумать формулу для рассчёта задержки (с учётом количества столов и количества рецептов/ингридиентов в наличии)

        int amountOfTables = tablesArray.childCount;
        int amountOfRecipes = 3;

        float result = 1 / (coefficient * (amountOfTables + amountOfRecipes));

        return result;
    }

}
