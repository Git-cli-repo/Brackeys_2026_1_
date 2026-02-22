using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;
using System.Collections;
using System.Linq;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
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
    public List<Image> inventoryImages;

    public Dictionary<ItemType, Sprite> sprReference;
    public InputActionReference inventoryEnableKey;

    public Dictionary<int, List<Item>> testList = new Dictionary<int, List<Item>>();
    public List<int> keys = new List<int>();
    public List<List<Item>> values = new List<List<Item>>();

    public int currentPage = 0;
    public int countTo9 = 0;
    public int displayPage = 0;

    public InputActionReference moveAction;
    float moveY = 0;

    public GameManager gameManager;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
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

        inventoryUI.SetActive(false);
        GameManager.Instance.inventoryActive = false;

        CreatePages();
    }

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

        inventoryUI.SetActive(false);
        GameManager.Instance.inventoryActive = false;

        CreatePages();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.inventoryActive)
        {
            keys = testList.Keys.ToList();
            values = testList.Values.ToList();
        }

        if (trigger)
        {
            UseItem(inventory[useItem]);
            trigger = false;
        }

        if (inventoryEnableKey.action.WasPressedThisFrame())
        {
            inventoryUI.SetActive(!GameManager.Instance.inventoryActive);
            GameManager.Instance.inventoryActive = !GameManager.Instance.inventoryActive;
            if(GameManager.Instance.inventoryActive) {  CreatePages(); displayPage = 0; LoadPage(); }
        }

        if (GameManager.Instance.inventoryActive && !cooldown)
        {
            Vector2 moveActionRead = moveAction.action.ReadValue<Vector2>();
            moveY = moveActionRead[0];

            if (moveY > 0)
            {                
                if (displayPage + 1 > testList.Keys.Count - 1)
                {
                    displayPage = 0;
                } else {
                    displayPage++;
                }
            } else if (moveY < 0) {                
                if (displayPage - 1 < 0)
                {
                    displayPage = testList.Keys.Count - 1;
                } else {
                    displayPage--;
                }
            }

            if(moveActionRead != new Vector2(0, 0)) LoadPage();
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

    public void AddItem(Item item)
    {
        inventory.Add(item);
        LoadPage();
    }

    public void CreatePages()
    {
        testList.Clear();
        countTo9 = 0;
        currentPage = 0;
        for(int i = 0; i < Mathf.Ceil((float)inventory.Count / 9); i++) testList[i] = new List<Item>();
        
        foreach (Item it in inventory)
        {
            Debug.Log($"Name {it.name}, countTo9 {countTo9}, currentPage {currentPage}");
            
            if(countTo9 + 1 >= 9)
            {
                countTo9 = 0;
                currentPage++;
            } else if (countTo9 < 9)
            {
                testList[currentPage].Add(it);
                countTo9++;
            }
        }

        Debug.Log($"TestList Key Count: {testList.Keys.Count}");
        Debug.Log($"TestList Value Count: {testList.Values.Count}");
        int counter = 0;
        foreach(List<Item> items in testList.Values)
        {
            Debug.Log($"TestList index {counter} Count: {items.Count}");
        }

        LoadPage();
    }

    public void LoadPage()
    {
        inventoryImages.ForEach(p => p.gameObject.SetActive(true));
        List<Sprite> spr = new List<Sprite>();
        spr = testList[displayPage].Select(p => !p.Encrypted ? sprReference[p.objectType] : encryptedImage).ToList();
        int g = 0;
        foreach(Image im in inventoryImages)
        {
            if(g < spr.Count) im.sprite = spr[g];
            else im.gameObject.SetActive(false);
            g++;
        }
        for(int i = 0; i < testList[displayPage].Count; i++)
        {
            if(!inventoryImages[i].TryGetComponent<ItemContainer>(out ItemContainer ic)){
                inventoryImages[i].gameObject.AddComponent<ItemContainer>().item = testList[displayPage][i];
                inventoryImages[i].gameObject.GetComponent<ItemContainer>().id = i;
            } else
            {
                ic.item = testList[displayPage][i];
                ic.id = i;
            }
        }
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);

        CreatePages();
    }

    public IEnumerator StartCooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(1f);
        cooldown = false;
    }

    // --------------------------- Pointer Logic -------------------------------
    public void PointerEnter(ItemContainer im)
    {
        string text = "";
        switch (im.item.objectType)
        {
          case ItemType.Weapon:
          text = $"Frequency: {im.item.attackFrequency}\nOffense Increase: {im.item.offenseIncrease}";
          break;
          case ItemType.Armor:
          text = $"Defense Increase: {im.item.defenseIncrease}";
          break;  
          case ItemType.Consumable:
          text = $"BDI: {im.item.baseDefenseIncrease}\nBOI: {im.item.baseOffenseIncrease}\nMax HP Inc.: {im.item.maxHpIncrease}\nHeal Amount: {im.item.hpRegain}";
          break;
          case ItemType.Room:
          text = $"Room ID: {im.item.roomID}";
          break;
        }

        inventoryUI.GetComponentInChildren<TooltipText>().gameObject.GetComponent<TMP_Text>().text = text; // keep note
        im.isSelected = true;
    }

    public void PointerExit(ItemContainer im)
    {
        im.isSelected = false;
        inventoryUI.GetComponentInChildren<TooltipText>().gameObject.GetComponent<TMP_Text>().text = "Hover over an item to see more details!";
    }

    public void PointerClick(ItemContainer im)
    {
        if (im.isSelected)
        {
            UseItem(im.item);
            CreatePages();
            displayPage = 0;
            LoadPage();
        } else
        {
            im.isSelected = true;
        }
    }
}
