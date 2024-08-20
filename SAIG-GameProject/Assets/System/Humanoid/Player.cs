using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : Humanoid
{
    [SerializeField] float dashDelay,dashingPower, dashingTime;
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType<Player>();
                if (objs.Length > 0)
                {
                    _instance = objs[0];
                }
                if (objs.Length > 1)
                {
                    Debug.LogError($"There are more than on {typeof(Player).Name} in scene.");
                }
                if (_instance == null)
                {
                    GameObject gob = new GameObject();
                    gob.hideFlags = HideFlags.HideAndDontSave;
                    _instance = gob.AddComponent<Player>();
                }
            }
            return _instance;
        }
    }
    TrailRenderer tr;
    Vector2 movement;
    float fireCooldown = 0;
    float defualtDamage;
    bool canDash = true;
    bool isDashing = false;
    public bool isDie { get; private set; }

    protected override void Start()
    {
        base.Start();
        tr = GetComponent<TrailRenderer>();
        Respawn();
    }
    public void Respawn()
    {
        defualtDamage = damage;
        
        isDie = false;
        HP = new Stat(health);
        SPD = new Stat(speed);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && fireCooldown <= 0)
        {
            Shoot();
            fireCooldown = 0.2f;
        }
        fireCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash && !isDashing) 
        {
            Debug.Log("Dash");
            StartCoroutine(Dash());
        }
        if(StageManager.Instance.IsGameAvalible)
        {
            Move();
        }
    }
    void Move()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + movement * SPD.CurrentStat * Time.fixedDeltaTime);
    }
    public override void TakeDamage(float stat)
    {
        base.TakeDamage(stat);
        animator.Play("Hurt");
        if(HP.CurrentStat == 0)
        {
            Die();
        }
    }
    IEnumerator Dash()
    {
        isDashing = true;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDirection = (mousePosition - (Vector2)transform.position).normalized;

        tr.emitting = true;
        rb.velocity = dashDirection * dashingPower;
        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;

        rb.velocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashDelay);
        canDash = true;
    }
    void Die()
    {
        Debug.Log("Die");
        damage = defualtDamage;
        StageManager.Instance.ResetGame();
    }
    public override void Heal(float stat)
    {
        base.Heal(stat);
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(StageManager.Instance.bulletPrefab,transform.position, Quaternion.identity);
        bullet.SetActive(true);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Make sure the z-coordinate is zero in 2D.

        bullet.GetComponent<Bullet>().CreateBullet(
            Bullet.UserType.Player, 
            mousePosition, 
            30f, 
            damage);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log("Hit Enemy");
            enemy.TakeDamage(2f);
            TakeDamage(0.5f);
        }
    }
    
}