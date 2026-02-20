using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;

public class InventoryManager : MonoBehaviour
{
    public bool trigger;
    public List<Item> inventory;
    PlayerController playerController;
    public Item equippedWeapon;
    public Item equippedArmor;
    public int useItem;

    public GameObject inventoryUI;
    public List<RawImage> inventoryImages;

    Dictionary<int, List<Item>> testList = new Dictionary<int, List<Item>>();

    public int currentPage = 0;
    public int countTo9 = 0;
    public int displayPage = 1;

    public InputActionReference moveAction;
    float moveY = 0;

    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        testList[0] = new List<Item>();

        CreatePages();
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            UseItem(inventory[useItem]);

            trigger = false;
        }

        if (inventoryUI.activeSelf)
        {
            Vector2 moveActionRead = moveAction.action.ReadValue<Vector2>();

            moveY = moveActionRead[0];

            if (moveY > 0)
            {
                displayPage += 1;
                if (displayPage > testList.Count)
                {
                    displayPage = 0;
                }
            }
            else if (moveY < 0)
            {
                displayPage -= 1;

                if (displayPage < 0)
                {
                    displayPage = testList.Count;
                }
            }
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
            }
            else if (item.objectType == ItemType.Room)
            {
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

    public void CreatePages()
    {
        foreach (Item it in inventory)
        {
            if (countTo9 < 9)
            {
                testList[currentPage].Add(it);
            }
            else
            {
                countTo9 = 0;
                currentPage++;
                testList[currentPage] = new List<Item>();
            }

            countTo9++;
        }
    }

    public void LoadPage()
    {
        for (int i = 0; i < 9; i++)
        {
            inventoryImages[i].texture = testList[displayPage][i].itemIcon;
        }
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);

        CreatePages();
    }

}
