using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class TowerController : EntityController, IDamageable
{
    private TargetFinder targetFinder;
    private EntityAttacker entityAttacker;

    private float activateWaitTime;

    private float lastAttackTime;

    private EntityType attackFilter;

    [SerializeField]
    public float health;

    private float hpConsumePerSec;


    private EntityController target;

    private void Awake()
    {
        targetFinder = GetComponent<TargetFinder>();
        entityAttacker = GetComponent<EntityAttacker>();
    }

    public override void Init(EntityData cardData, Vector3 point, Team team)
    {
        base.Init(cardData, point, team);

        if (cardData is TowerData tower)
        {
            this.cardData = tower;

                health = tower.DefenseData.health;

                attackFilter = tower.AttackData.attackFilter;
        }

        activateWaitTime = this.cardData.activateWaitTime;
        if ((cardData as TowerData).lifeTime > 0)
            hpConsumePerSec = health / (cardData as TowerData).lifeTime;
    }

    private void Update()
    {
        CheckTransition();
        ExecuteState();
    }

    public void TakeDamage(float damage)
    {
        if (health < 0) return;

        health -= damage;

        if (health <= 0)
        {
            Dead();
        }
    }
    public void TakeDamage(float damage, Team team)
    {
        if (this.team == team) return;
        if (health < 0) return;

        health -= damage;

        // Debug.Log($"{team}타워 피격! 남은체력 {health}");

        if (health <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        if (cardData.SpecialData != null && cardData.SpecialData.hasDeathrattle)
        {
            CardArrangementManager.Instance.Arrangement(cardData.SpecialData.deathrattleEntity, team, transform.position);
        }

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
                if (entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange, out _)) { ChangeState(EntityState.Attack); }
                break;
            case EntityState.Attack:
                if (target == null || !entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange, out _)) { ChangeState(EntityState.LookingForTarget); }
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
    }
    private void ExecuteState()
    {
        if (state != EntityState.Idle)
        {
            if (cardData.SpecialData != null && cardData.SpecialData.hasSummon)
            {
                if (Time.time - lastSummonTime > cardData.SpecialData.summonInterval)
                {
                    CardArrangementManager.Instance.Arrangement(cardData.SpecialData.summonEntity, team, transform.position);
                    lastSummonTime = Time.time;
                }
            }

            TakeDamage(hpConsumePerSec * Time.deltaTime);
        }

        switch (state)
        {
            case EntityState.Idle:
                runningCoroutine = null;
                activateWaitTime -= Time.deltaTime;
                break;
            case EntityState.LookingForTarget:
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
                    StartCoroutine(entityAttacker.CoAttack(cardData.AttackData, modelPosition.position, target, team));
                    lastAttackTime = Time.time;
                }
                if (target != null)
                        transform.LookAt(target.transform.position);
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
    }
    private void ChangeState(EntityState state)
    {
        switch (state)
        {
            case EntityState.Idle:
                break;
            case EntityState.LookingForTarget:
                break;
            case EntityState.Attack:
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

        this.state = state;
    }
}
