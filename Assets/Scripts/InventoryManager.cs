using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;
using System.Collections;
using System.Linq;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool trigger;
    bool cooldown = false;
    public List<Item> inventory;
    PlayerController playerController;
    public Item equippedWeapon;
    public Item equippedArmor;
    public int useItem;
    public Sprite weaponImage;
    public Sprite roomImage; 
    public Sprite encryptedImage;
    public Sprite consumableImage;
    public Sprite armourImage;
    public Sprite emailImage;
    public GameObject inventoryUI;
    public List<Sprite> inventoryImages;

    Dictionary<ItemType, Sprite> sprReference;

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

        sprReference = new Dictionary<ItemType, Sprite>
        {
            {ItemType.Weapon, weaponImage},
            {ItemType.Armor, armourImage},
            {ItemType.Consumable, consumableImage},
            {ItemType.Email, emailImage},
            {ItemType.Room, roomImage}
        };

        inventoryUI.GetComponent<Tooltip>().gameObject.SetActive(false);


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

        if (inventoryUI.activeSelf && !cooldown)
        {
            Vector2 moveActionRead = moveAction.action.ReadValue<Vector2>();
            moveY = moveActionRead[0];

            if (moveY > 0)
            {
                DampenLogic();
                
                displayPage += 1;
                if (displayPage > testList.Count)
                {
                    displayPage = 0;
                }
            }
            else if (moveY < 0)
            {
                DampenLogic();

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
                GameManager.Instance.inDialogueMode = true;
                GameManager.Instance.dialogue = item.contents;
                // Skipping DMat - Keep note
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
        inventoryImages = testList[displayPage].Select(p => !p.Encrypted ? sprReference[p.objectType] : encryptedImage).ToList();
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);

        CreatePages();
    }

    public IEnumerator DampenLogic()
    {
        cooldown = true;
        yield return new WaitForSeconds(1);
        cooldown = false;
    }

    // --------------------------- Pointer Logic -------------------------------
    public void OnPointerEnter(PointerEventData pdata)
    {
        // pdata.hovered[0].GetComponent<ItemContainer>();
    }

    public void OnPointerExit(PointerEventData pdata)
    {
        
    }

}
