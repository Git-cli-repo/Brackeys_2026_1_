using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

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

    public bool stopMoving;
    public float waitAfterAttack;
    public float knockbackForce;
    public float hitboxEnableDistance;
    public float hitboxEnableCoolDownTime;
    public float hitboxEnableCoolDownTimer;

    public List<Transform> enemies;

    [SerializeField] private Material whiteFlashMaterial;
    private Material originalMaterial;

    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitbox = hitboxGO.GetComponent<Hitbox>();
        offense = baseOffense;
        defense = baseDefense;
        hp = maxHp;
        attackCoolDownTimer = attackCoolDownTime;
        hitboxEnableCoolDownTimer = hitboxEnableCoolDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        hitboxEnableCoolDownTimer -= Time.deltaTime;

        if (hitboxEnableCoolDownTimer <= 0)
        {
            enemies = GameObject.FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Select(p => p.GetComponent<Transform>()).ToList();

            foreach (Transform enemy in enemies)
            {
                Vector3 distance = transform.position - enemy.position;
                if (distance.magnitude <= hitboxEnableDistance)
                {
                    hitboxGO.SetActive(true);
                    break;
                }
                hitboxGO.SetActive(false);
            }
            
            hitboxEnableCoolDownTimer = hitboxEnableCoolDownTime;
        }

        Vector2 moveActionRead = moveAction.action.ReadValue<Vector2>();

        if (moveActionRead[0] > 0)
            hitboxGO.transform.localPosition = new Vector3(hitboxLocationOffset, 0f, 0f);
        else if (moveActionRead[0] < 0)
            hitboxGO.transform.localPosition = new Vector3(-hitboxLocationOffset, 0f, 0f);
        else if (moveActionRead[1] > 0)
            hitboxGO.transform.localPosition = new Vector3(0f, hitboxLocationOffset, 0f);
        else if (moveActionRead[1] < 0)
            hitboxGO.transform.localPosition = new Vector3(0f, -hitboxLocationOffset, 0f);

        attackCoolDownTimer -= Time.deltaTime;

        float attacking = attackAction.action.ReadValue<float>();
        if (attacking > 0f && attackCoolDownTimer <= 0f)
        {
            attackCoolDownTimer = attackCoolDownTime;
            Debug.Log("Player Attacked");
            SwordAttack();
        }
    }

    public void Damage(float damage, Vector3 source)
    {
        hp -= damage * (1f - defense);

        if (hp >= 0f)
        {
            // Die animation
            // Fade into black
            // teleport to save point
            // regain hp
            // Respawn enemies
        }

        Vector2 direction = (transform.position - source).normalized;
        StartCoroutine(KnockbackRoutine(direction, 0.1f));

        FlashWhite();
    }

    public void SwordAttack()
    {
        stopMoving = true;

        foreach (GameObject GO in hitbox.detectedObjects)
        {
            detectedEnemy = GO.GetComponent<EnemyController>();

            detectedEnemy.Damage(baseOffense);
            Debug.Log("Damaged enemy");

            StartCoroutine(WaitAfterAttack());
        }
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;
    }


    public IEnumerator WaitAfterAttack()
    {
        yield return new WaitForSeconds(waitAfterAttack);

        stopMoving = false;
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
}
