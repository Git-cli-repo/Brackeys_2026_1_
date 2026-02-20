using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Projectile,
    Dash,
    Environment,
    Room,
    Email
}

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class EnemyAttack : ScriptableObject
{
    public AttackType attackType;

    [Header("Projectile Attack Options")]
    public GameObject Projectile;
    public GameObject ProjectileSpawnPoint;

    [Header("Barrage Attack Options")]
    public float barrageAttackInterval;
    public int AmountOfAttacks = 1;

    [Header("Misc")]
    public float AttackInterval;
    public float attackDistance;
}
