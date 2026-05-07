using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackEntityController : RootController
{
    public AttackData attackData;
    public Transform modelPosition;

    private EntityAttacker entityAttacker;
    private EntityMover entityMover;

    private float activateWaitTime;
    private bool isAttackEnd = false;
    private EntityController target;
    private IDamageable targetDamageComponent;

    private bool isNonTarget;

    private Vector3 destination;




    public void Init(AttackData attackData, Vector3 point, EntityController target, Team team)
    {
        if (attackData.attackModel != null)
            Instantiate(attackData.attackModel, modelPosition);

        activateWaitTime = attackData.attackArriveTime;
        this.attackData = attackData;
        transform.position = point;
        transform.forward = target.transform.position - transform.position;
        this.team = team;
        isNonTarget = attackData.isNonTarget;

        if (isNonTarget)
        {
            destination = target.transform.position;
        }
        else
        {
            this.target = target;
        }

        if (attackData.attackType == AttackType.Single)
        {
            targetDamageComponent = target.GetComponent<IDamageable>();
        }
        else if (attackData.attackType == AttackType.Area)
        {

        }

        entityMover = GetComponent<EntityMover>();
        entityAttacker = GetComponent<EntityAttacker>();
    }
    public void Init(AttackData attackData, Vector3 point, Vector3 target, Team team)
    {
        if (attackData.attackModel != null)
            Instantiate(attackData.attackModel, modelPosition);

        activateWaitTime = attackData.attackArriveTime;
        this.attackData = attackData;
        transform.position = point;
        transform.forward = target - transform.position;
        this.team = team;
        isNonTarget = attackData.isNonTarget;

        if (isNonTarget)
        {
            destination = target;
        }
        else
        {
            Debug.LogError("isNonTarget 에러");
        }

        if (attackData.attackType == AttackType.Single)
        {
            Debug.LogError("attackType 에러");
        }
        else if (attackData.attackType == AttackType.Area)
        {

        }

        entityMover = GetComponent<EntityMover>();
        entityAttacker = GetComponent<EntityAttacker>();
    }


    private void Update()
    {
        CheckTransition();
        ExecuteState();
    }

    private void CheckTransition()
    {
        switch (state)
        {
            case EntityState.Idle:
                if (activateWaitTime < 0f) ChangeState(EntityState.LookingForTarget);
                break;
            case EntityState.LookingForTarget:
                if (!isNonTarget)
                {
                    if (target != null)
                        destination = target.transform.position;
                    if (entityAttacker.IsTargetInRange(destination, 0.5f, out _)) ChangeState(EntityState.Attack);
                }
                else
                {
                    if (entityAttacker.IsTargetInRange(destination, 0.5f, out _)) ChangeState(EntityState.Attack);
                }
                if (attackData.projectileSpeed <= 0f) ChangeState(EntityState.Attack);
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
    }
    private void ExecuteState()
    {
        if (target != null) destination = target.transform.position;

        switch (state)
        {
            case EntityState.Idle:
                activateWaitTime -= Time.deltaTime;
                break;
            case EntityState.LookingForTarget:
                if (target == null) isNonTarget = true;
                if (isNonTarget)
                    entityMover.AttackMoveTo(transform.position, destination, attackData.projectileSpeed);
                else
                    entityMover.AttackMoveTo(transform.position, target, attackData.projectileSpeed);
                break;
            case EntityState.Attack:
                if (!isAttackEnd)
                {
                    isAttackEnd = true;
                    StartCoroutine(CoAttack());
                }
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


    IEnumerator CoAttack()
    {
        transform.position = target == null ? destination : target.modelPosition.transform.position;

        yield return new WaitForSeconds(attackData.attackArriveTime);

        if (attackData.attackArriveModel != null)
        {
            var model = Instantiate(attackData.attackArriveModel, modelPosition.position, Quaternion.identity);
            Destroy(model, 0.1f);
        }

        switch (attackData.attackType)
        {
            case AttackType.Single:
                if (targetDamageComponent != null && target != null)
                {
                    targetDamageComponent.TakeDamage(attackData.damage, team);
                }
                break;
            case AttackType.Area:
                List<EntityController> enemies = null;

                if (team == Team.RedTeam)
                {
                    enemies = EntityManager.blueTeamEntities;
                }
                else if (team == Team.BlueTeam)
                {
                    enemies = EntityManager.redTeamEntities;
                }

                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    float sqrDistance = (enemies[i].transform.position - transform.position).sqrMagnitude;
                    float radius = attackData.attackRadius + enemies[i].size;
                    if (sqrDistance < radius * radius)
                    {
                        if (enemies[i].cardData.DefenseData != null)
                        {
                            enemies[i].GetComponent<IDamageable>().TakeDamage(attackData.damage, team);
                        }
                    }
                }
                break;
        }

        float afterTime = attackData.attackDuration <= 0 ? 0.1f : attackData.attackDuration;
        yield return new WaitForSeconds(afterTime);
        Destroy(gameObject);
    }
        //private void Update()
        //{
        //    if (activateWaitTime > 0f)
        //    {
        //        activateWaitTime -= Time.deltaTime;
        //        return;
        //    }

        //    Move();

        //    if (isMoveEnd && !isAttackEnd)
        //        Attack();
        //}

        //protected override void Move()
        //{
        //    if (attackData.projectileSpeed <= 0 || distance == 0)
        //    {
        //        transform.position = destination;
        //        isMoveEnd = true;
        //        return;
        //    }
        //    else
        //    {
        //        transform.position = Vector3.Lerp(OriginPoint, destination, moveProgress / distance);
        //        moveProgress += attackData.projectileSpeed * Time.deltaTime;

        //        if (moveProgress > distance)
        //            isMoveEnd = true;
        //    }
        //}
        //protected override void LockOn()
        //{
        //    throw new NotImplementedException();
        //}
        //protected override void Attack()
        //{
        //    isAttackEnd = true;
        //    if (attackData != null && runningCoroutine == null)
        //    {
        //        runningCoroutine = StartCoroutine(CoAttack());
        //    }
        //}
        //IEnumerator CoAttack()
        //{
        //    yield return new WaitForSeconds(attackData.attackArriveTime);

        //    switch (attackData.attackType)
        //    {
        //        case AttackType.Single:
        //            if (target != null)
        //            {
        //                target.TakeDamage(attackData.damage, team);
        //            }
        //            break;
        //        case AttackType.Area:
        //            List<EntityController> enemies = null;

        //            if (team == Team.RedTeam)
        //            {
        //                enemies = EntityManager.blueTeamEntities;
        //            }
        //            else if (team == Team.BlueTeam)
        //            {
        //                enemies = EntityManager.redTeamEntities;
        //            }

        //            for (int i = enemies.Count - 1; i >= 0; i--)
        //            {
        //                float sqrDistance = (enemies[i].transform.position - transform.position).sqrMagnitude;
        //                if (sqrDistance < attackData.attackRadius * attackData.attackRadius)
        //                {
        //                    if (enemies[i].cardData.DefenseData != null)
        //                    {
        //                        enemies[i].TakeDamage(attackData.damage, team);
        //                    }
        //                }
        //            }
        //            break;
        //    }

        //    if (attackData.attackArriveModel != null)
        //        Instantiate(attackData.attackArriveModel, transform);

        //    runningCoroutine = null;

        //    yield return new WaitForSeconds(0.1f);

        //    Destroy(gameObject);
        //}

        //protected override void CheckAttackRange()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void TakeDamage(float damage, Team team)
        //{
        //    throw new NotImplementedException();
        //}
    
}
