using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform player;
    public float maxSpeed = 5f;
    public float detectionDistance;
    public float slowDownDistance = 3f;
    public EnemyController enemy;
    public Rigidbody2D body;

    void Start()
    {
        enemy.enabled = false;
        player = GameObject.FindFirstObjectByType<PlayerMovement>().gameObject.transform;
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;
        Debug.Log(distance);

        if (distance > detectionDistance)
        {    
            enemy.enabled = false;
            body.linearVelocity = new Vector2(0, 0);
        }
        else
        {
            enemy.enabled = true;
        }

        if (enemy.canMove && distance < detectionDistance)
        {
            if (player == null)
            {
                return;
            }
            
            Vector2 moveDirection = direction.normalized;

            float t = Mathf.InverseLerp(0, slowDownDistance, distance);
            float currentSpeed = Mathf.Lerp(0f, maxSpeed, t);
            body.linearVelocity = moveDirection * currentSpeed;
        }
    }
}
