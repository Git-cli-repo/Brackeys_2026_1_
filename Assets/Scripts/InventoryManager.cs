using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public bool trigger;
    public List<Item> inventory;
    PlayerController playerController;
    public Item equippedWeapon;
    public Item equippedArmor;
    public int useItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            UseItem(inventory[useItem]);

            trigger = false;
        }
    }

    public void UseItem(Item item)
    {
        if (!item.Encrypted)
        {
            if (item.objectType == ItemType.Weapon)
            {
                inventory.Add(equippedWeapon);

                equippedWeapon = item;
                playerController.offense += item.offenseIncrease;
                playerController.attackFrequency = item.attackFrequency;

            }
            else if (item.objectType == ItemType.Armor)
            {
                inventory.Add(equippedArmor);

                equippedArmor = item;
                playerController.defense += item.defenseIncrease;
            }
            else if (item.objectType == ItemType.Consumable)
            {
                if (item.statIncrease)
                {
                    playerController.baseDefense += item.baseDefenseIncrease;
                    playerController.baseOffense += item.baseOffenseIncrease;

                    playerController.maxHp += item.maxHpIncrease;
                }
                else if (item.heal)
                {
                    playerController.hp += item.hpRegain;
                }

            }
            else if (item.objectType == ItemType.Email)
            {
                //Dialogue
            } else if(item.objectType == ItemType.Room) {
                GameManager.Instance.UseRoom(item);
            }
            else
            {
                //Dialogue saying item can't be used
            }
        }
        else
        {
            return;
        }
       
        inventory.Remove(item);
    }

    public void GetItem(Item item)
    {
        inventory.Add(item);
    }
}
