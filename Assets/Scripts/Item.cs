using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Encrypted,
    Email
}

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemType objectType;

    [Space]

    [Header("Weapon Options")]
    public float attackFrequency;
    public float offenseIncrease; // if Melee
    public bool isRanged;
    public GameObject projectile;

    [Space]

    [Header("Armor Options")]
    public float defenseIncrease;

    [Space]

    [Header("Consumable Options")]
    public bool statIncrease;
    public float baseOffenseIncrease;
    public float baseDefenseIncrease;
    public float maxHpIncrease;
    public bool heal;
    public float hpRegain;

    [Space]

    [Header("Encrypted Options")]
    public Item decryptedItem;

    //[Header("Email Options")]
    // insert system to read email in dialogue box
}
