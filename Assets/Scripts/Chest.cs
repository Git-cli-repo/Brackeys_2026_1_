using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Chest")]
public class Chest : ScriptableObject
{
    public int chestID;
    public List<Item> contents;
}