using UnityEngine;


public class Dish : MonoBehaviour
{
    protected Dish() { }

    public Rigidbody2D RigidbodyComponent { get; set; }

    void Awake()
    {
        RigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    public void Land() 
    {
        Collider2D[] collidedObjects = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (Collider2D collider in collidedObjects)
        {
            if (collider.transform.name[0] == 'T')
            {
                RigidbodyComponent.velocity = Vector2.zero;
                transform.localScale = new Vector3(1.125f, 1.125f, 1.125f);
                transform.position = collider.transform.position;
                return;
            }
        }
        Destroy(gameObject);
    }
}
