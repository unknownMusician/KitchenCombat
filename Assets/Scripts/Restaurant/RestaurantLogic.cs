using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

public class RestaurantLogic : MonoBehaviour 
{
    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    private Vector2 mouseDownPos;
    private float mouseDownTime;

    private GameObject dish;

    public InspectorValues inspectorValues = new InspectorValues();
    [System.Serializable] public class InspectorValues 
    {
        public Transform customersArrayMenu;
        public Transform tablesArrayMenu;
        public Transform restaurantMenu;
        public Transform dishesMenu;

        public Transform startPoint;
        public Transform exitPoint;
        public Transform waitPoint;
    }

    void Start() 
    {
        StartCoroutine(CustomerSpawning());
        dish = Instantiate(GameLogic.Prefabs.dish,
            new Vector2(-3.25f, -7f),
            Quaternion.identity,
            inspectorValues.dishesMenu
            );
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            mouseDownTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            var localDir = (Vector2)Input.mousePosition - mouseDownPos;
            if (Time.time - mouseDownTime < 0.05f || localDir.magnitude < 30)
            {
                return;
            }
            else
            {
                dish.rigidbodyComponent.mass = 1;
                dish.rigidbodyComponent.AddForce(localDir);
            }
        }
    }

    private IEnumerator CustomerSpawning() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(CalculateDelay() + Random.Range(0, 5000) / 1000);
            SpawnCustomer();
        }
    }

    [SerializeField] private float delayCoefficient = default;
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
        Instantiate(GameLogic.Prefabs.customer,
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
}
