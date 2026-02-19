using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public float speedX;
    public float speedY;
    public InputActionReference moveAction;
    public Animator abbeyAnimator;
    public float moveX;
    public float moveY;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        abbeyAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveActionRead = moveAction.action.ReadValue<Vector2>();
        
        if(!GameManager.Instance.inDialogueMode) body.linearVelocity = new Vector2(moveActionRead[0] * speedX, moveActionRead[1] * speedY);
            if(body.linearVelocity == new Vector2(0, 0)) abbeyAnimator.speed = 0;
            else { 
                abbeyAnimator.speed = 1;
                abbeyAnimator.SetFloat("moveX", moveActionRead[0]);
                abbeyAnimator.SetFloat("moveY", moveActionRead[1]);
            }
            moveX = abbeyAnimator.GetFloat("moveX");
            moveY = abbeyAnimator.GetFloat("moveY");
            speed = abbeyAnimator.speed;
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<DMat>(out DMat dialogue))
        {
            GameManager.Instance.inDialogueMode = true;
            GameManager.Instance.dialogue = dialogue.dialogue;
        }
    }
}
