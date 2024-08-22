using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceKillObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 20f);
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
            Debug.Log("Set InstanceKill");
            Destroy(gameObject);
            player.UpdateBerserkMode();
        }
    }
}
