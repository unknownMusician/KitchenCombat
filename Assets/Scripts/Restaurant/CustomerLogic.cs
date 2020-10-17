using UnityEngine;

public class CustomerLogic : MonoBehaviour {

    [SerializeField]
    public GameObject customerPrefab;
    [SerializeField]
    public GameObject customersArray;

    public void SpawnCustomer() {
        Instantiate(customerPrefab, 
            new Vector2(0, 0), 
            Quaternion.identity, 
            customersArray.GetComponent<Transform>()
            );
    }

}
