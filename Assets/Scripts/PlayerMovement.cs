using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public float speedX;
    public float speedY;
    public InputActionReference moveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveActionRead = moveAction.action.ReadValue<Vector2>();
        body.linearVelocity = new Vector2(moveActionRead[0] * speedX, moveActionRead[1] * speedY);

        
    }
}
