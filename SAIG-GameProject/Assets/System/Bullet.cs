using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    float speed = 50f;
    float damage; // The amount of damage the bullet will deal
    public float lifeTime = 2f; // The time after which the bullet is destroyed
    Vector2 direction;
    private Rigidbody2D rb;
    [SerializeField] Light2D light;
    public enum UserType
    {
        Player = 0,
        Enemy = 1
    }
    UserType user;
    public void CreateBullet(UserType type, Vector3 target, float speed, float damage)
    {
        user = type;
        this.speed = speed;
        this.damage = damage;
        Destroy(gameObject, lifeTime);
        
        // Set the velocity of the bullet
        direction = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    public void CreateBullet(UserType type, Vector3 target, float speed, float damage, Color color)
    {
        user = type;
        this.speed = speed;
        this.damage = damage;
        Destroy(gameObject, lifeTime);

        light.color = color;
        // Set the velocity of the bullet
        direction = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    void Update()
    {
        Vector3 movement = direction * speed *  1000 * Time.deltaTime;
        rb.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(user == UserType.Player)
        {
            if (hitInfo.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if(user == UserType.Enemy)
        {
            if (hitInfo.TryGetComponent<Player>(out Player player))
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}

