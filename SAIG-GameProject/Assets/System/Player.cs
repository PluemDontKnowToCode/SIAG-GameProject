using Unity.VisualScripting;
using UnityEngine;

public class Player : Humanoid
{
    [SerializeField] GameObject bulletPrefab;
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

    Vector2 movement;

    protected override void Start()
    {
        base.Start();
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
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
        if(HP.CurrentStat == 0)
        {
            Die();
        }
    }
    void Die()
    {

    }
    public override void Heal(float stat)
    {
        base.Heal(stat);
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab,transform.position, Quaternion.identity);
        bullet.SetActive(true);
    }
}