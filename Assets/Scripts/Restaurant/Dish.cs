using UnityEngine;
using System.Collections;


public class Dish : MonoBehaviour
{
    public bool IsThrown { get; set; }
    public bool IsLanded { get; set; }
    public IEnumerator coroutine { get; set; }

    private void Awake() 
    {
        IsThrown = false;
        IsLanded = false;
    }

    public void Land() 
    {
        Collider2D[] collidedObjects = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (Collider2D collider in collidedObjects)
        {
            if (collider.transform.name[0] == 'T')
            {
                StopCoroutine(coroutine);
                transform.localScale = new Vector3(1.125f, 1.125f, 1.125f);
                transform.position = collider.transform.position;
                IsLanded = true;
                return;
            }
        }
        Destroy(gameObject);
    }

    public IEnumerator Throw(Vector2 dir) 
    {
        IsThrown = true;
        float xDelta = dir.x / 2000;
        float yDelta = dir.y / 2000;
        while (true) 
        {
            yield return new WaitForSeconds(0.0125f);
            transform.position = new Vector2(transform.position.x + xDelta, transform.position.y + yDelta);
            xDelta -= xDelta * 0.001f;
            yDelta -= yDelta * 0.001f;
        }
    }
}
