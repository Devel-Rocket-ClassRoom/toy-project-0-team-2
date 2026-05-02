using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class TowerController : EntityController, IDamageable
{
    private NavMeshAgent agent;

    private float activateWaitTime;
    private bool isLockOn;

    private float damage;
    private float crownTowerDamage;
    private float attackInterval = 0f;
    private float rangeSqr;

    private EntityType attackTarget;
    private AttackType attackType;

    private float health;
    private float hpComsumePerSecond;


    private EntityController NearestTarget
    {
        get
        {
            var entities = team == Team.RedTeam ? EntityManager.blueTeamEntities : EntityManager.redTeamEntities;

            EntityController result = null;
            float min = float.MaxValue;

            float rangeSqr = cardData.AttackData.sightRange * cardData.AttackData.sightRange;
            foreach (var entity in entities)
            {
                if ((attackTarget & entity.entityType) == 0) { continue; }
                if (entity == this) { continue; }

                Vector3 diff = entity.transform.position - transform.position;
                diff.y = 0;

                if (diff.sqrMagnitude <= rangeSqr)
                {
                    if (diff.sqrMagnitude < min)
                    {
                        min = diff.sqrMagnitude;
                        result = entity;
                    }
                }
            }

            return result;
        }
    }
    private EntityController target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Init(CardData cardData, Vector3 point)
    {
        base.Init(cardData, point);

        if (cardData is TowerData unit)
        {
            this.cardData = unit;

            damage = unit.AttackData.damage;
            health = unit.DefenseData.health;

            rangeSqr = unit.AttackData.attackRange * unit.AttackData.attackRange;
            attackTarget = unit.AttackData.attackFilter;

            hpComsumePerSecond = unit.lifeTime > 0 ? health / unit.lifeTime : 0;
        }

        activateWaitTime = this.cardData.activateWaitTime;
        EntityManager.onEntitiesChanged += LockOn;
    }

    private void Update()
    {
        if (activateWaitTime > 0f)
        {
            activateWaitTime -= Time.deltaTime;
            // 이곳에서 타이머UI 갱신
            return;
        }

        if (Time.time - lastSearchTime > searchInterval)
        {
            LockOn();
            lastSearchTime = Time.time;
        }

        if (target != null)
        {
            CheckAttackRange();

            if (attackInterval < 0f)
            {
                Attack();
            }
        }

        attackInterval -= Time.deltaTime;
        TakeDamage(hpComsumePerSecond * Time.deltaTime);
    }

    protected override void Move()
    {
        throw new NotImplementedException();
    }
    protected override void LockOn()
    {
        target = NearestTarget;
    }

    protected override void CheckAttackRange()
    {
        Vector3 diff = target.transform.position - transform.position;
        diff.y = 0;
        isLockOn = diff.sqrMagnitude < rangeSqr;
    }

    protected override void Attack()
    {
        if (isLockOn && runningCoroutine == null)
        {
            runningCoroutine = StartCoroutine(CoAttack());
            attackInterval = cardData.AttackData.attackInterval;
        }
    }

    IEnumerator CoAttack()
    {
        yield return new WaitForSeconds(cardData.AttackData.firstAttackTime);

        switch (attackType)
        {
            case AttackType.Single:
                if (target == null) break;
                target.transform.GetComponent<IDamageable>().TakeDamage(damage, team);
                break;
            case AttackType.Area:
                // 데미지를 주는 오브젝트 생성
                break;
        }

        yield return new WaitForSeconds(cardData.AttackData.lastDelay);
        runningCoroutine = null;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Dead();
        }
    }

    public override void TakeDamage(float damage, Team team)
    {
        if (this.team == team) return;
        
        TakeDamage(damage);
        Debug.Log($"{team} 타워피격! 남은체력 {health}");
    }

    private void Dead()
    {
        EntityManager.onEntitiesChanged -= LockOn;
        EntityManager.RemoveEntities(this);
        Destroy(gameObject);
    }
}
