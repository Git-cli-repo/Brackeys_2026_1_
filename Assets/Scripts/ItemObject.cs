using UnityEngine;

public class ItemObject : MonoBehaviour
{
    InventoryManager inventoryManager;
    public Item item;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inventoryManager.AddItem(item);

            //Animation/sound

            Destroy(gameObject);
        }
    }
}
