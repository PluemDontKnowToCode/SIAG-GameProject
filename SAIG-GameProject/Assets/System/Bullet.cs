using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifeTime = 2f; // The time after which the bullet is destroyed
    public int damage = 10; // The amount of damage the bullet will deal
    Vector2 direction;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Make sure the z-coordinate is zero in 2D.

        // Set the velocity of the bullet
        direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    void Update()
    {
        Vector3 movement = direction * speed *  1000 * Time.deltaTime;
        rb.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // If the target has an Enemy script, deal damage to it
        if (hitInfo.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

