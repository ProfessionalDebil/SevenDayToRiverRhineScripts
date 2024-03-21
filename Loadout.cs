using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Loadout {
    [Header("Inventory")]
    public List<Item> primary = new List<Item>(7);
    public List<Item> underbarrel = new List<Item>(12);
    public List<Item> secondary = new List<Item>(6);
    public List<Item> launcher = new List<Item>(3);
    public List<Item> others = new List<Item>(38);
}