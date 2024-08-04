using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Item
{
    public ItemData data;
    public int amount;
}
public class InventoryManager : SingletonPersistent<InventoryManager>
{
    [Header("Inventory")]
    List<Item> items = new List<Item>();

    
    
}

