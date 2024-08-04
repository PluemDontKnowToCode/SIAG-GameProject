using Unity.VisualScripting;
using UnityEngine;

public class Player : Humanoid
{
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

    [SerializeField] int health = 5;
    Enemy currentTarget;

        protected override void Start()
    {
        base.Start();
        HP = new Stat(health);
    }
    public override void TakeDamage(int stat)
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
    public override void Heal(int stat)
    {
        base.Heal(stat);
    }
    void Kill()
    {
        //typing
    }
    void LockTarget()
    {

    }
}