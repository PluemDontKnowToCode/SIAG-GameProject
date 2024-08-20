using UnityEngine;
using System;

[RequireComponent(typeof(Animator),typeof(Rigidbody2D))]
public class Humanoid : MonoBehaviour
{
    public Stat HP;
    public Stat SPD;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected Collider2D collider;
    [SerializeField] protected float health = 5f;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    
    protected virtual void Start()
    {
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }
    
    public virtual void TakeDamage(float stat)
    {
        HP.UpdateStat(-1f * stat);
    }

    public virtual void Heal(float stat)
    {
        HP.UpdateStat(stat);
    }
}

[Serializable]
public class Stat
{
    private float _maxStat;
    private float _currentStat;

    public float CurrentStat => _currentStat;
    public float MaxStat => _maxStat;

    // Constructor to set up Stat
    public Stat(float maxHP)
    {
        _currentStat = _maxStat = maxHP;
    }

    // Method to heal and hurt character
    public float UpdateStat(float amount)
    {
        _currentStat += amount;
        if (_currentStat > _maxStat)
        {
            _currentStat = _maxStat;
        }
        else if (_currentStat < 0f)
        {
            _currentStat = 0f;
        }
        return _currentStat;
    }

    public void SetStat(float newStat)
    {
        if (newStat > _maxStat)
        {
            _currentStat = _maxStat;
        }
        else if (newStat < 0f)
        {
            _currentStat = 0f;
        }
        else
        {
            _currentStat = newStat;
        }
    }
}


