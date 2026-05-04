using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class UnitController : EntityController, IDamageable
{
    private NavMeshAgent agent;

    private UnitData unitData;
    private float activateWaitTime;
    private bool isLockOn;

    private float damage;
    private float crownTowerDamage;
    private float attackInterval = 0f;

    private EntityType attackTarget;
    private AttackType attackType;

    private float health;


    private TowerController NearestCrownTower
    {
        get
        {
            var towers = team == Team.RedTeam ? EntityManager.blueTeamCrownTower : EntityManager.redTeamCrownTower;

            TowerController result = null;
            float min = float.MaxValue;

            foreach (TowerController tower in towers)
            {
                if (tower == this) continue;
                if ((tower.entityType & EntityType.CrownTower) == 0) continue;

                Vector3 diff = tower.transform.position - transform.position;
                diff.y = 0;

                if (diff.sqrMagnitude < min)
                {
                    min = diff.sqrMagnitude;
                    result = tower;
                }
            }

            return result;
        }
    }
    private EntityController NearestTarget
    {
        get
        {
            var entities = team == Team.RedTeam ? EntityManager.blueTeamEnntities : EntityManager.redTeamEntities;

            EntityController result = null;
            float min = float.MaxValue;

            foreach (var entity in entities)
            {
                if ((attackTarget & entity.entityType) == 0) { Debug.Log($"공격타입 같지 않음 : {attackTarget} / {entity.entityType}"); continue; }
                if (entity == this) { continue; }

                float range = unitData.AttackData.sightRange + size + entity.size;
                Vector3 diff = entity.transform.position - transform.position;
                diff.y = 0;

                if (diff.sqrMagnitude <= range * range)
                {
                    if (diff.sqrMagnitude < min)
                    {
                        min = diff.sqrMagnitude;
                        result = entity;
                    }
                }
            }

            if (result == null)
            {
                result = NearestCrownTower;
            }

            return result;
        }
    }
    private EntityController target;

    private Coroutine runningCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Init(CardData cardData)
    {
        base.Init(cardData);

        if (cardData is UnitData unit)
        {
            unitData = unit;

            damage = unit.AttackData.damage;
            health = unit.DefenseData.health;

            attackTarget = unit.AttackData.attackTarget;
        }

        activateWaitTime = unitData.activateWaitTime;
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
            Move();
            CheckAttackRange();

            if (attackInterval < 0f)
            {
                Attack();
            }
        }

        attackInterval -= Time.deltaTime;
    }

    protected override void Move()
    {
        agent.isStopped = isLockOn;
        if (isLockOn)
        {
            return;
        }

        agent.SetDestination(target.transform.position);
    }
    protected override void LockOn()
    {
        target = NearestTarget;
    }

    protected override void CheckAttackRange()
    {
        Vector3 diff = target.transform.position - transform.position;
        diff.y = 0;

        float range = unitData.AttackData.attackRange + size + target.size;

        isLockOn = diff.sqrMagnitude < range * range;
    }

    protected override void Attack()
    {
        if (isLockOn && runningCoroutine == null)
        { 
            runningCoroutine = StartCoroutine(CoAttack());
            attackInterval = unitData.AttackData.attackInterval;
        }
    }

    IEnumerator CoAttack()
    {
        yield return new WaitForSeconds(unitData.AttackData.firstAttackTime);

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

        yield return new WaitForSeconds(unitData.AttackData.lastDelay);
        runningCoroutine = null;
    }

    public void TakeDamage(float damage, Team team)
    {
        if (this.team == team) return;

        health -= damage;

        Debug.Log($"{team} 피격! 남은체력 {health}");

        if (health <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        EntityManager.onEntitiesChanged -= LockOn;
        EntityManager.RemoveEntities(this);
        Destroy(gameObject);
    }
}
