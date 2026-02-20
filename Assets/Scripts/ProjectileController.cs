using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 5f;
    [SerializeField] float damage = 10f;
    [SerializeField] float knockbackForce = 2f;
    [SerializeField] bool aimAtPlayer = false;
    [SerializeField] float launchAngle = 0f;
    private Vector2 direction;

    void Start()
    {
        transform.parent = null;
        
        rb = GetComponent<Rigidbody2D>();

        if (aimAtPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
            else
            {
                direction = Vector2.right; // fallback
            }
        }
        else
        {
            float angleRad = launchAngle * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;
        }

        rb.linearVelocity = direction * speed;

        // Rotate the sprite to face the direction
        float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.Damage(damage, transform.position);
            }

            Destroy(gameObject);
        }
    }
}