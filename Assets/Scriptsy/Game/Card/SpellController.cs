using UnityEngine;
using System;

public class SpellController : EntityController
{
    private SpellData spellData;
    private float activateWaitTime;

    public override void Init(CardData cardData)
    {
        if (cardData is SpellData unit)
        {
            spellData = unit;
        }

        activateWaitTime = spellData.activateWaitTime;
    }

    private void Update()
    {
        if (activateWaitTime > 0f)
        {
            activateWaitTime -= Time.deltaTime;
            return;
        }

        Attack();
    }

    protected override void Move()
    {
        throw new NotImplementedException();
    }
    protected override void LockOn()
    {
        throw new NotImplementedException();
    }
    protected override void Attack()
    {

    }

    protected override void CheckAttackRange()
    {
        throw new NotImplementedException();
    }
}
