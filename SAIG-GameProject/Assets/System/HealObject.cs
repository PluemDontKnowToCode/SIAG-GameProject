using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!StageManager.Instance.IsGameAvalible)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("Healing");
            Destroy(gameObject);
            player.Heal(5f);
        }
    }
}
