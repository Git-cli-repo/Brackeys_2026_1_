using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform player;
    public float maxSpeed = 5f;
    public float DetectionDistance;
    public float slowDownDistance = 3f;
    public EnemyController enemy;

    void Start()
    {
        enemy.enabled = false;
        player = GameObject.FindFirstObjectByType<PlayerMovement>().gameObject.transform;
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;
        Debug.Log(distance);

        if (distance > DetectionDistance)
        {    
            return;
        }
        else
        {
            enemy.enabled = true;
        }

        if (enemy.canMove)
        {
            if (player == null)
            {
                return;
            }
            
            Vector2 moveDirection = direction.normalized;

            float t = Mathf.InverseLerp(0, slowDownDistance, distance);
            float currentSpeed = Mathf.Lerp(0f, maxSpeed, t);
            transform.position += (Vector3)(moveDirection * currentSpeed * Time.deltaTime);
        }
    }
}
