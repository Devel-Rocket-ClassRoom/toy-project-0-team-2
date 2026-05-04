using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    [Header("유닛이 사용할 값")]
    public float attackInterval; // 공격 간격 (선후딜 총합보다 짧으면 0으로 두기)
    public float firstAttackTime; // 선딜레이
    public float lastDelay; // 후딜레이
    public float sightRange; // 시야
    public float attackRange; // 사거리

    [Header("공격에서 사용할 값")]
    public float damage;
    public float crownTowerDamage;
    public float attackArriveTime; // 공격 도착 이후 대기 시간 (예 : 마법병 떨구는 시간)
    public float projectileSpeed; // 투사체 속도
    public float attackRadius; // 공격 범위 (AttackType : Area 한정)

    public bool isNonTarget;
    public bool isSingleTarget;
    public bool toKingTower;

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