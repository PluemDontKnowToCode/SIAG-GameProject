using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Enemy : Humanoid
{
    float shootTime = 0;
    [SerializeField] float score = 300;
    [SerializeField] float fireCooldown;
    [Header("RandomMovement")]
    [SerializeField] private float moveDuration = 0.5f; // How long the movement lasts
    [SerializeField] private float idleDuration = 1f;   // How long to wait before the next movement
    [SerializeField] private float movementRadius = 3f; // Radius of the random movement

    [SerializeField] private float knockbackForce = 10f; // The force applied during knockback
    [SerializeField] private float knockbackDuration = 0.2f; // How long the knockback lasts


    private bool isKnockedBack = false;
    private bool isMoving;
    [SerializeField] AudioSource dieSFX;
    [SerializeField] AudioSource ShootSFX;
    [SerializeField] AudioSource HitSFX;
    protected override void Start()
    {
        base.Start();
        StageManager.Instance.EnemyCount++;
        HP = new Stat(health + StageManager.Instance.killCount/ 20);
        SPD = new Stat(speed);
    }
    public void TakeDamage(float stat,Vector2 direction)
    {
        base.TakeDamage(stat);
        
        HitSFX.Play();
        
        if(HP.CurrentStat == 0)
        {
            StageManager.Instance.EnemyCount--;
            StageManager.Instance.Score += (int)score;
            StageManager.Instance.killCount++;
            Destroy(gameObject);
        }
        else
        {
            ApplyKnockback(direction);
        }
    }
    void DropItem()
    {

    }
    void Update()
    {
        if(!StageManager.Instance.IsGameAvalible)
        {
            Destroy(gameObject);
        }
        Move();
        score -= Time.deltaTime;
        shootTime += Time.deltaTime;
        if(shootTime >= fireCooldown)
        {
            Debug.Log("Shoot");
            shootTime = 0;
            Shoot();
        }
    }
    void Move()
    {
        float distance = Vector3.Distance(
            Player.Instance.transform.position,
            transform.position
        );
        if(distance > 6)
        {
            Vector2 newPosition = Vector2.MoveTowards(
                rb.position,
                Player.Instance.transform.position,
                SPD.MaxStat * Time.deltaTime);

            rb.MovePosition(newPosition);
        }
        else
        {
            StartCoroutine(MoveRandomly());
        }
    }
    IEnumerator MoveRandomly()
    {
        if (!isMoving)
        {
            Vector2 randomDirection = GetRandomDirection();
            Vector2 targetPosition = (Vector2)transform.position + randomDirection;

            isMoving = true;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, SPD.MaxStat * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isMoving = false;
            yield return new WaitForSeconds(idleDuration);
        }
    }
    public void ApplyKnockback(Vector2 direction)
    {
        if (!isKnockedBack)
        {
            StartCoroutine(KnockbackCoroutine(direction));
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;

        // Apply the knockback force
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        // Stop the knockback by setting the velocity to zero
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }
    private Vector2 GetRandomDirection()
    {
        // Generate a random point within a radius
        return Random.insideUnitCircle * movementRadius;
    }
    void Shoot()
    {
        ShootSFX.Play();

        GameObject bullet = Instantiate(StageManager.Instance.bulletPrefab,transform.position, Quaternion.identity);
        bullet.SetActive(true);

        bullet.GetComponent<Bullet>().CreateBullet(
            Bullet.UserType.Enemy,
            Player.Instance.transform.position,
            8f,
            damage,
            color
            );
    }
    void OnTriggerEnter2D()
    {
        
    }
}
