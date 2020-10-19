using UnityEngine;

public class CustomerLogic : MonoBehaviour 
{
    private Transform transformComponent;
    private Rigidbody2D rigidbodyComponent;

    void Awake() 
    {
        transformComponent = transform;
        rigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    void Start() 
    {
        Debug.Log(transformComponent.position);
    }
}
