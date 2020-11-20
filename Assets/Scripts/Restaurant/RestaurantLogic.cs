using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.ObjectModel;


public class RestaurantLogic : MonoBehaviour 
{
    public Transform GameMenu { get; set; } = default;
    public RectTransform UIMenu { get; set; } = default;

    private Vector2 mouseDownPos;
    private float mouseDownTime;

    private Dish dish;

    public InspectorValues inspectorValues = new InspectorValues();
    public InputManager inputManager = default;
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
        dish = Instantiate(GameLogic.Prefabs.dish,
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
    }

    public class InputManager {
        protected RestaurantLogic r;
        public InputManager(RestaurantLogic r) {
            this.r = r;
            UI.instance.OnViewChange += CheckView;
            coroutine = null;
        }

        public UnityAction<Vector2> OnSwipe = default;
        public UnityAction OnTap = default;

        protected Coroutine coroutine = default;
        protected Vector2 mouseDownPos = default;
        protected float mouseDownTime = default;

        public void CheckView(bool isKitchen) {
            if (!isKitchen) {
                if (coroutine == null) { coroutine = r.StartCoroutine(WaitForPress()); }
            } else {
                if (coroutine != null) {
                    r.StopCoroutine(coroutine);
                    coroutine = null;
                }
            }
        }
        public IEnumerator WaitForPress() {
            while (true) {
                if (Input.GetMouseButtonDown(0)) {
                    mouseDownPos = Input.mousePosition;
                    mouseDownTime = Time.time;
                    coroutine = r.StartCoroutine(WaitForRelease());
                    break;
                }
                yield return null;
            }
        }
        public IEnumerator WaitForRelease() {
            while (true) {
                if (Input.GetMouseButtonUp(0)) {
                    var localDir = (Vector2)Input.mousePosition - mouseDownPos;
                    if (Time.time - mouseDownTime < 0.05f || localDir.magnitude < 30) {
                        OnTap?.Invoke();
                    } else {
                        OnSwipe?.Invoke(localDir);
                    }
                    coroutine = r.StartCoroutine(WaitForPress());
                    break;
                }
                yield return null;
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
