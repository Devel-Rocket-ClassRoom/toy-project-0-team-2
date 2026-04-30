using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public float damage;
    public float crownTowerDamage;
    public float attackInterval;
    public float firstAttackTime;
    public float lastDelay;
    public float loadTime;
    public float sightRange;
    public float attackRange;
    public float knuckbackTile;
    public float freezeTime;

    public EntityType attackTarget;
    public AttackType attackType;

    public GameObject attackModel;
}


public enum AttackType
{
    Single, Area,
}