using UnityEngine;

public class UnitController : EntityController, IDamageable
{
    private TargetFinder targetFinder;
    private EntityMover entityMover;
    private EntityAttacker entityAttacker;
    private Animator animator;

    private float activateWaitTime;

    private float lastAttackTime;

    private EntityType attackFilter;

    public float health { get; set; }
    private float speed;
    private float continuousMoveTime = 0f;


    [SerializeField]
    private EntityController target;

    private void Awake()
    {
        targetFinder = GetComponent<TargetFinder>();
        entityMover = GetComponent<EntityMover>();
        entityAttacker = GetComponent<EntityAttacker>();
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
        entityMover?.Init(team);

        animator = modelPosition.GetComponentInChildren<Animator>();
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
                if (entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange, out _))
                {
                    ChangeState(EntityState.Attack);
                }
                if (cardData.SpecialData != null && cardData.SpecialData.hasCharge)
                {
                    if (entityAttacker.IsTargetInRange(target, cardData.SpecialData.maxChargeRange, out float sqrDistance))
                    {
                        float minDistance = cardData.SpecialData.minChargeRange;

                        if (sqrDistance > minDistance * minDistance)
                            ChangeState(EntityState.PrepareCharge);
                    }
                }
                if (cardData.SpecialData != null
                    && cardData.SpecialData.hasSprint
                    && continuousMoveTime > cardData.SpecialData.springPrepareTime)
                { ChangeState(EntityState.Sprint); }
                break;

            case EntityState.Attack:
                if (target == null || !entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange, out _)) { ChangeState(EntityState.LookingForTarget); }
                break;

            case EntityState.Sprint:
                if (target != null && entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange, out _))
                {
                    runningCoroutine = StartCoroutine(entityAttacker.CoAttack(cardData.SpecialData.sprintAttack, modelPosition.position, target, team));
                    ChangeState(EntityState.Attack);
                }
                break;

            case EntityState.PrepareCharge:
                if (Time.time - lastChargeTime > cardData.SpecialData.chargePrepareTime)
                {
                    ChangeState(EntityState.Charge);
                }
                if (target == null) ChangeState(EntityState.LookingForTarget);
                break;

            case EntityState.Charge:
                if (target != null && entityAttacker.IsTargetInRange(target, cardData.AttackData.attackRange, out _))
                {
                    runningCoroutine = StartCoroutine(entityAttacker.CoAttack(cardData.SpecialData.chargeAttack, modelPosition.position, target, team));
                    ChangeState(EntityState.Attack);
                }
                else if (target == null)
                {
                    ChangeState(EntityState.LookingForTarget);
                }
                break;

            case EntityState.Dead:
                break;
        }
    }

    private void ExecuteState()
    {
        if (state != EntityState.Idle && cardData.SpecialData != null && cardData.SpecialData.hasSummon)
        {
            if (Time.time - lastSummonTime > cardData.SpecialData.summonInterval)
            {
                CardArrangementManager.Instance.Arrangement(cardData.SpecialData.summonEntity, team, transform.position);
                lastSummonTime = Time.time;
            }
        }

        switch (state)
        {
            case EntityState.Idle:
                runningCoroutine = null;
                activateWaitTime -= Time.deltaTime;
                break;

            case EntityState.LookingForTarget:
                entityMover.UnitMoveTo(target, speed);
                continuousMoveTime += Time.deltaTime;
                if (Time.time - lastSearchTime > searchInterval || EntityManager.isEntityUpdated)
                {
                    target = targetFinder?.FindNearestTarget(team, attackFilter, cardData.AttackData.sightRange);
                    lastSearchTime = Time.time;
                }
                break;

            case EntityState.Attack:
                if (Time.time - lastAttackTime > cardData.AttackData.attackInterval)
                {
                    animator.SetTrigger("Attack");
                    StartCoroutine(entityAttacker.CoAttack(cardData.AttackData, modelPosition.position, target, team));
                    lastAttackTime = Time.time;
                }
                if (target != null)
                {
                    var look = target.modelPosition.position;
                    look.y = transform.position.y;
                    transform.LookAt(look);
                }
                break;

            case EntityState.Sprint:
                entityMover.UnitMoveTo(target, cardData.SpecialData.sprintSpeed);
                if (Time.time - lastSearchTime > searchInterval)
                {
                    target = targetFinder.FindNearestTarget(team, attackFilter, cardData.AttackData.sightRange);
                    lastSearchTime = Time.time;
                }
                break;

            case EntityState.PrepareCharge:
                if (Time.time - lastSearchTime > searchInterval)
                {
                    target = targetFinder.FindNearestTarget(team, attackFilter, cardData.AttackData.sightRange);
                    lastSearchTime = Time.time;
                }
                break;

            case EntityState.Charge:
                if (target != null)
                    entityMover.UnitMoveTo(target, cardData.SpecialData.chargeSpeed);
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
                animator.SetTrigger("Idle");
                break;

            case EntityState.LookingForTarget:
                animator.SetTrigger("Move");
                entityMover.MoveControl(false);
                break;

            case EntityState.Attack:
                entityMover.MoveControl(true);
                break;

            case EntityState.Sprint:
                break;

            case EntityState.PrepareCharge:
                entityMover.MoveControl(true);
                lastChargeTime = Time.time;
                break;

            case EntityState.Charge:
                entityMover.MoveControl(false);
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