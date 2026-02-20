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

    public float attackCoolDownTime;

    public GameObject hitboxGO;
    private Hitbox hitbox;
    public int hitboxLocationOffset;

    public InputActionReference moveAction;
    public InputActionReference attackAction;
    public float attackCoolDownTimer;

    public EnemyController detectedEnemy;

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
            hitboxGO.transform.localPosition = new Vector3(hitboxLocationOffset, 0, 0);
        else if (moveActionRead[0] < 0)
            hitboxGO.transform.localPosition = new Vector3(-hitboxLocationOffset, 0, 0);
        else if (moveActionRead[1] > 0)
            hitboxGO.transform.localPosition = new Vector3(0, hitboxLocationOffset, 0);
        else if (moveActionRead[1] < 0)
            hitboxGO.transform.localPosition = new Vector3(0, -hitboxLocationOffset, 0);

        attackCoolDownTimer -= Time.deltaTime;


        float attacking = attackAction.action.ReadValue<float>();
        if (attacking > 0f)
            Debug.Log("Player Attacked");
            SwordAttack();

    }
    
    public void SwordAttack()
    {
        if (attackCoolDownTimer <= 0)
        {
            attackCoolDownTimer = attackCoolDownTime;

            foreach (GameObject GO in hitbox.detectedObjects)
            {
                if (GO.tag == "Enemy")
                {
                    detectedEnemy = GO.GetComponent<EnemyController>();

                    detectedEnemy.Damage(baseOffense);
                }
            }
        }
    }
}
