using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class UnitController : EntityController, IDamageable
{
    private NavMeshAgent agent;

    private float activateWaitTime;
    private bool isLockOn;

    private float attackInterval = 0f;

    private EntityType attackFilter;

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
            var entities = team == Team.RedTeam ? EntityManager.blueTeamEntities : EntityManager.redTeamEntities;

            EntityController result = null;
            float min = float.MaxValue;

            foreach (var entity in entities)
            {
                if ((attackFilter & entity.entityType) == 0) { Debug.Log($"공격타입 같지 않음 : {attackFilter} / {entity.entityType}"); continue; }
                if (entity == this) { continue; }

                float range = cardData.AttackData.sightRange + size + entity.size;
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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Init(CardData cardData, Vector3 point)
    {
        base.Init(cardData, point);

        if (cardData is UnitData unit)
        {
            this.cardData = unit;

            health = unit.DefenseData.health;

            attackFilter = unit.AttackData.attackFilter;
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
            Move();
            CheckAttackRange();

            if (attackInterval < 0f)
            {
                Attack();
            }
        }
        else
        {
            isLockOn = false;
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
        if (!isLockOn)
            target = NearestTarget;
    }

    protected override void CheckAttackRange()
    {
        Vector3 diff = target.transform.position - transform.position;
        diff.y = 0;
        float range = cardData.AttackData.attackRange + size + target.size;

        isLockOn = diff.sqrMagnitude < range * range;
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
        if (cardData == null || cardData.AttackData == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(cardData.AttackData.firstAttackTime);

        if (target == null)
        {
            yield break;
        }

        GameObject entity = Instantiate(CardArrangementManager.spell.gameObject);
        var controller = entity.GetComponent<SpellController>();
        controller.team = team;
        controller.Init(cardData, transform.position, target.transform.position, transform.forward);
        controller.target = target;


        yield return new WaitForSeconds(cardData.AttackData.lastDelay);
        runningCoroutine = null;
    }

    public override void TakeDamage(float damage, Team team)
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
        StopAllCoroutines();
        EntityManager.onEntitiesChanged -= LockOn;
        EntityManager.RemoveEntities(this);
        Destroy(gameObject);
    }
}
