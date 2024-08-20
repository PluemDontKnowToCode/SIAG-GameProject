using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Enemy : Humanoid
{
    float shootTime = 0;
    [SerializeField] float score = 300;
    [SerializeField] float fireCooldown;
    protected override void Start()
    {
        base.Start();
        StageManager.Instance.EnemyCount++;
        HP = new Stat(health + StageManager.Instance.killCount/ 20);
        SPD = new Stat(speed);
    }
    public override void TakeDamage(float stat)
    {
        base.TakeDamage(stat);
        if(HP.CurrentStat == 0)
        {
            StageManager.Instance.EnemyCount--;
            StageManager.Instance.Score += (int)score;
            Destroy(gameObject);
        }
    }
    void SpawnItem()
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
                SPD.CurrentStat * Time.deltaTime);

            rb.MovePosition(newPosition);
        }
    }
    public override void Heal(float stat)
    {
        base.Heal(stat);
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(StageManager.Instance.bulletPrefab,transform.position, Quaternion.identity);
        bullet.SetActive(true);

        Vector3 randomPosition = new Vector3(
            Random.Range(-0.8f, 0.8f),
            Random.Range(-0.8f, 0.8f),
            0
        );

        bullet.GetComponent<Bullet>().CreateBullet(
            Bullet.UserType.Enemy,
            Player.Instance.transform.position + randomPosition,
            10f,
            damage,
            Color.red
            );
    }
    void OnTriggerEnter2D()
    {
        
    }
}
