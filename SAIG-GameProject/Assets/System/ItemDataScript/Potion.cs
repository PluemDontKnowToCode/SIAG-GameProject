using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemData", menuName = "new Item/Potion", order = 1)]
public class Potion : ItemData
{
    int heal;
    public override bool Use()
    {
        Player.Instance.Heal(heal);
        return true;
    }
}
