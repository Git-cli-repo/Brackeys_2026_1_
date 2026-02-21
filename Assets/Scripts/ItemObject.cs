using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InventoryManager.Instance.AddItem(item);

            //Animation/sound

            Destroy(gameObject);
        }
    }
}
