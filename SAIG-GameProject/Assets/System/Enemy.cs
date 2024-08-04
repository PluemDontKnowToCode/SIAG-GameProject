using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Humanoid
{
    string word;
    protected override void Start()
    {
        base.Start();
        HP = new Stat(word.Length);
    }
    public override void TakeDamage(int stat)
    {
        base.TakeDamage(stat);
    }
    public override void Heal(int stat)
    {
        base.Heal(stat);
    }
    void SetUp()
    {
        
    }
}
