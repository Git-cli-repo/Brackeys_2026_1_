using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float baseOffense;
    public float baseDefense;
    public float maxHp;
    public float attackFrequency;

    public float offense;
    public float defense;
    public float hp;

    public GameObject hitbox;
    public int hitboxLocationOffset;

     public InputActionReference moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offense = baseOffense;
        defense = baseDefense;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveActionRead = moveAction.action.ReadValue<Vector2>();

        if (moveActionRead[0] > 0)
            hitbox.transform.localPosition = new Vector3(hitboxLocationOffset, 0, 0);
        if (moveActionRead[0] < 0)
           hitbox.transform.localPosition = new Vector3(hitboxLocationOffset, 0, 0);
        
    }
    

}
