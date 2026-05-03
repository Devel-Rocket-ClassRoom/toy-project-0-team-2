using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class UnitController : EntityController, IDamageable
{
    [SerializeField]
    private EntityState state;

    private TargetFinder targetFinder;
    private EntityMover entityMover;
    private EntityAttacker entityAttacker;
    private NavMeshAgent agent;

    private float activateWaitTime;
    private bool isLockOn;

    private float lastAttackTime;

    private EntityType attackFilter;

    private float health;
    private float speed;
    private float continuousMoveTime = 0f;


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
        targetFinder = GetComponent<TargetFinder>();
        entityMover = GetComponent<EntityMover>();
        entityAttacker = GetComponent<EntityAttacker>();
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Init(EntityData cardData, Vector3 point, Team team)
    {
        base.Init(cardData, point, team);

        if (cardData is UnitData unit)
        {
            this.cardData = unit;

            health = unit.DefenseData.health;

            attackFilter = unit.AttackData.attackFilter;
        }

        activateWaitTime = this.cardData.activateWaitTime;
        speed = (cardData as UnitData).tilePerSeconds;
    }

    private void Update()
    {
        CheckTransition();
        ExecuteState();
    }

    public void TakeDamage(float damage, Team team)
    {
        if (this.team == team) return;
        if (health < 0) return;

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
        EntityManager.RemoveEntities(this);
        Destroy(gameObject);
    }



    private void CheckTransition()
    {
        switch (state)
        {
            case EntityState.Idle:
                if (activateWaitTime < 0) { ChangeState(EntityState.LookingForTarget); }
                break;
            case EntityState.LookingForTarget:
                if (entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange)) { ChangeState(EntityState.Attack); }
                if (cardData.SpecialData != null
                    && cardData.SpecialData.hasSprint
                    && continuousMoveTime > cardData.SpecialData.springPrepareTime)
                    { ChangeState(EntityState.Sprint); }
                break;
            case EntityState.Attack:
                if (target == null || !entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange)) { ChangeState(EntityState.LookingForTarget); }
                break;
            case EntityState.Sprint:
                if (target != null && entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange))
                {
                    runningCoroutine = StartCoroutine(entityAttacker.CoAttack(cardData.AttackData, target, team));
                    ChangeState(EntityState.Attack);
                }
                break;
            case EntityState.PrepareCharge:
                break;
            case EntityState.Charge:
                break;
            case EntityState.Dead:
                break;
        }
    }
    private void ExecuteState()
    {
        switch (state)
        {
            case EntityState.Idle:
                runningCoroutine = null;
                activateWaitTime -= Time.deltaTime;
                break;
            case EntityState.LookingForTarget:
                entityMover.MoveTo(target, speed);
                continuousMoveTime += Time.deltaTime;
                if (Time.time - lastSearchTime > searchInterval || EntityManager.isEntityUpdated)
                {
                    target = targetFinder?.FindNearestTarget(team, attackFilter, cardData.AttackData.sightRange);
                    if (target == null) target = targetFinder?.FindNearestCrownTower(team, attackFilter);
                    lastSearchTime = Time.time;
                }
                break;
            case EntityState.Attack:
                if (Time.time - lastAttackTime > cardData.AttackData.attackInterval)
                {
                    StartCoroutine(entityAttacker.CoAttack(cardData.AttackData, target, team));
                    lastAttackTime = Time.time;
                }
                if (target != null)
                    transform.LookAt(target.transform.position);
                break;
            case EntityState.Sprint:
                entityMover.MoveTo(target, cardData.SpecialData.sprintSpeed);
                if (Time.time - lastSearchTime > searchInterval)
                {
                    targetFinder.FindNearestTarget(team, attackFilter, cardData.AttackData.sightRange);
                    lastSearchTime = Time.time;
                }
                break;
            case EntityState.PrepareCharge:
                break;
            case EntityState.Charge:
                break;
            case EntityState.Dead:
                if (cardData.SpecialData != null && cardData.SpecialData.hasDeathrattle)
                {

                }
                break;
        }
    }
    private void ChangeState(EntityState state)
    {
        switch (state)
        {
            case EntityState.Idle:
                break;
            case EntityState.LookingForTarget:
                entityMover.MoveControl(false);
                break;
            case EntityState.Attack:
                entityMover.MoveControl(true);
                break;
            case EntityState.Sprint:
                break;
            case EntityState.PrepareCharge:
                break;
            case EntityState.Charge:
                break;
            case EntityState.Dead:
                break;
        }

        continuousMoveTime = 0f;
        this.state = state;
    }
}


public enum EntityState
{
    Idle,
    LookingForTarget,
    Attack,
    Sprint,
    PrepareCharge,
    Charge,
    Dead,
}