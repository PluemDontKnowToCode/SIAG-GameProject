using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Humanoid
{
    string word;
    float shootTime;
    protected override void Start()
    {
        base.Start();
    }
    public override void TakeDamage(float stat)
    {
        base.TakeDamage(stat);
        if(HP.CurrentStat == 0)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        shootTime += Time.deltaTime;
    }
    public override void Heal(float stat)
    {
        base.Heal(stat);
    }
}
