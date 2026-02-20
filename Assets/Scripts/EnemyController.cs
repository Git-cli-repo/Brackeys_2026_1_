using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    public float defense;
    public float contactDamage;
    public float knockbackForce = 2f;
    public float nextAttackWaitTime;
    public float nextAttackWaitTimer;
    public bool isPlayerSighted;
    public float playerSightedDistance;

    [Space]

    [SerializeField] private List<EnemyAttack> attacks;
    public List<EnemyAttack> possibleAttacks = new List<EnemyAttack>();

    [Space]

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Material whiteFlashMaterial;
    private Material originalMaterial;
    private float flashDuration = 0.1f;
    public bool canMove = true;
    private EnemyAttack selectedAttack;
    private GameObject player;
    private Transform playerTransform;
    private PlayerController playerController;


    public bool isAttacking = false;
    public Hitbox hitbox;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalMaterial = spriteRenderer.material;

        if (player != null)
        {
            playerTransform = player.transform;
            playerController = player.GetComponent<PlayerController>();
        }

        nextAttackWaitTimer = nextAttackWaitTime;
    }

    private void Update()
    {
        if ((transform.position - player.transform.position).magnitude <= playerSightedDistance && !isPlayerSighted)
        {
            isPlayerSighted = true;
         }

        if (health <= 0f)
        {
            Die();
            return;
        }

        if (hitbox.detectedObject)
        {
            playerController.Damage(contactDamage, transform.position);

            Vector2 direction = (transform.position - player.transform.position).normalized;
            StartCoroutine(KnockbackRoutine(direction, 0.1f));
        }

        if (!isAttacking)
        {
            if (selectedAttack == null && isPlayerSighted && nextAttackWaitTimer <= 0)
            {
                SelectAttack();
            }

            if (selectedAttack != null && !isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackBarrage(selectedAttack));
            }
        }

        nextAttackWaitTimer -= Time.deltaTime;
    }

    private void Die()
    {
        //particle effect

        Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        health -= damage * (1 - defense);

        StartCoroutine(FlashWhite());

        canMove = false;

        Vector2 direction = (transform.position - player.transform.position).normalized;
        StartCoroutine(KnockbackRoutine(direction, 0.1f));

        canMove = true;
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float duration)
    {
        float elapsed = 0f;

        Vector3 start = transform.position;
        Vector3 target = start + new Vector3(knockbackForce * direction.x, knockbackForce * direction.y, 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }
    }

    private void SelectAttack()
    {
        possibleAttacks.Clear();

        foreach (EnemyAttack attack in attacks)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);

            if (distance <= attack.attackDistance)
                possibleAttacks.Add(attack);

        }

        if (possibleAttacks.Count > 0)
        {
            int index = Random.Range(0, possibleAttacks.Count);
            selectedAttack = possibleAttacks[index];
        }
    }

    private IEnumerator AttackBarrage(EnemyAttack attack)
    {
        isAttacking = true;
        canMove = false;

        yield return new WaitForSeconds(attack.AttackInterval);

        for (int i = 0; i < attack.AmountOfAttacks; i++)
        {
            if (attack.attackType == AttackType.Projectile)
            {
                Instantiate(attack.Projectile, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(attack.barrageAttackInterval);
        }

        selectedAttack = null;
        canMove = true;
        isAttacking = false;
        nextAttackWaitTimer = nextAttackWaitTime;

        yield break;
    }
}
