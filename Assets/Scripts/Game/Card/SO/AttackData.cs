using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    [Header("유닛이 사용할 값")]
    public float attackInterval;
    public float firstAttackTime;
    public float lastDelay;
    public float loadTime;
    public float sightRange; // 시야
    public float attackRange; // 사거리

    [Header("공격에서 사용할 값")]
    public float damage;
    public float crownTowerDamage;
    public float knuckbackTile;
    public float freezeTime;
    public float attackArriveTime; // 공격 도착 이후 대기 시간 (예 : 마법병 떨구는 시간)
    public float projectileSpeed; // 투사체 속도
    public float attackRadius; // 공격 범위 (AttackType : Area 한정)

    public EntityType attackFilter;
    public AttackType attackType;

    [Header("모델")]
    public GameObject attackModel;
    public GameObject attackArriveModel;
}


public enum AttackType
{
    Single, Area,
}