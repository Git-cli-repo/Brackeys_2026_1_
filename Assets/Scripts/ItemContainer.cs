using UnityEngine;
using UnityEngine.EventSystems;

public class ItemContainer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Item item;
    public bool isSelected = false;
    public int id;

    public void OnPointerEnter(PointerEventData pdata)
    {
        InventoryManager.Instance.PointerEnter(this);
    }

    public void OnPointerClick(PointerEventData pdata)
    {
        InventoryManager.Instance.PointerClick(this);
    }

    public void OnPointerExit(PointerEventData pdata)
    {
        
    }
}