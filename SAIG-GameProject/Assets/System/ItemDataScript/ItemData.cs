using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemData", menuName = "new Item/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public string Name;
    [TextArea(3,100)] public string Description;
    public Sprite sprite;
    public virtual bool Use()
    {
        Debug.Log("Use Item");
        return false;
    }
}
