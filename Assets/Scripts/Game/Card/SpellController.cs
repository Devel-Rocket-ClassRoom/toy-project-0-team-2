using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpellController : EntityController
{
    private float activateWaitTime;
    private bool isMoveEnd = false;
    private bool isAttackEnd = false;
    public EntityController target;

    private Vector3 OriginPoint; 
    private Vector3 destination;
    private float distance;
    private float moveProgress = 0f;

    public override void Init(CardData cardData, Vector3 point) // 카드가 직접 사용하는 Init
    {
        OriginPoint = team == Team.RedTeam ? EntityManager.redTeamCrownTower[0].transform.position : EntityManager.blueTeamCrownTower[0].transform.position;
        base.Init(cardData, OriginPoint, cardData.AttackData.attackModel);
        destination = point;
        distance = Vector3.Distance(OriginPoint, destination);

        if (cardData is SpellData spell)
        {
            this.cardData = spell;
        }

        activateWaitTime = this.cardData.activateWaitTime;
    }

    public void Init(CardData cardData, Vector3 point, Vector3 destination, Vector3 forward) // 유닛이 사용하는 Init
    {
        OriginPoint = point;
        base.Init(cardData, OriginPoint, cardData.AttackData.attackModel);

        this.destination = destination;
        distance = Vector3.Distance(OriginPoint, destination);

        if (cardData is SpellData spell)
        {
            this.cardData = spell;
        }

        transform.forward = forward;
    }

    private void Update()
    {
        if (activateWaitTime > 0f)
        {
            activateWaitTime -= Time.deltaTime;
            return;
        }

        Move();

        if (isMoveEnd && !isAttackEnd)
            Attack();
    }

    protected override void Move()
    {
        if (cardData.AttackData.projectileSpeed <= 0 || distance == 0)
        {
            transform.position = destination;
            isMoveEnd = true;
            return;
        }
        else
        {
            transform.position = Vector3.Lerp(OriginPoint, destination, moveProgress / distance);
            moveProgress += cardData.AttackData.projectileSpeed * Time.deltaTime;

            if (moveProgress > distance)
                isMoveEnd = true;
        }
    }
    protected override void LockOn()
    {
        throw new NotImplementedException();
    }
    protected override void Attack()
    {
        isAttackEnd = true;
        if (cardData.AttackData != null && runningCoroutine == null)
        {
            runningCoroutine = StartCoroutine(CoAttack());
        }
    }
    IEnumerator CoAttack()
    {
        yield return new WaitForSeconds(cardData.AttackData.attackArriveTime);

        switch (cardData.AttackData.attackType)
        {
            case AttackType.Single:
                if (target != null)
                {
                    target.TakeDamage(cardData.AttackData.damage, team);
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
                    if (sqrDistance < cardData.AttackData.attackRadius * cardData.AttackData.attackRadius)
                    {
                        if (enemies[i].cardData.DefenseData != null)
                        {
                            enemies[i].TakeDamage(cardData.AttackData.damage, team);
                        }
                    }
                }
                break;
        }

        if (cardData.AttackData.attackArriveModel != null)
            Instantiate(cardData.AttackData.attackArriveModel, transform);

        runningCoroutine = null;

        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }

    protected override void CheckAttackRange()
    {
        throw new NotImplementedException();
    }

    public override void TakeDamage(float damage, Team team)
    {
        throw new NotImplementedException();
    }
}
