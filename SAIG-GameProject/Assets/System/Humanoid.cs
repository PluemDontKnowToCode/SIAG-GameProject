using UnityEngine;
using System;
[RequireComponent(typeof(Animator),typeof(Rigidbody2D),typeof(Collider2D))]
public class Humanoid : MonoBehaviour
{
    [SerializeField] string _name;
    public string Name => _name;
    public Stat HP;
    protected Animator animator;
    protected Rigidbody2D rigidBody2D;
    protected Collider2D collider;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }
    public virtual void TakeDamage(int stat)
    {
        HP.UpdateStat(-1 * stat);
        Debug.Log("Take Damage");
    }
    public virtual void Heal(int stat)
    {
        HP.UpdateStat(stat);
        Debug.Log("Heal");
    }
}

[Serializable]
public class Stat
{
    [SerializeField] private int _maxStat;
    private int _currentStat;

    public int CurrentStat => _currentStat;
    public int MaxStat => _maxStat;

    // Constructor to set up Stat
    public Stat(int maxHP)
    {
        _currentStat = _maxStat = maxHP;
    }

    // Method to heal and hurt character
    public int UpdateStat(int amount)
    {
        _currentStat += amount;
        if (_currentStat > _maxStat)
        {
            _currentStat = _maxStat;
        }
        else if (_currentStat < 0)
        {
            _currentStat = 0;
        }
        return _currentStat;
    }

    public void SetStat(int newStat)
    {
        if (newStat > _maxStat)
        {
            _currentStat = _maxStat;
        }
        else if (newStat < 0)
        {
            _currentStat = 0;
        }
        else
        {
            _currentStat = newStat;
        }
    }
}

