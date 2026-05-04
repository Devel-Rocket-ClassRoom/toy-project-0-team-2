using UnityEngine;
using System.Collections;

public class EntityAttacker : MonoBehaviour
{
    public bool IsTargetInRange(EntityController target, float attackRange)
    {
        if (target == null) return false;
        return IsTargetInRange(target.transform.position, attackRange + target.size);
    }

    public bool IsTargetInRange(Vector3 target, float attackRange)
    {
        Vector3 diff = target - transform.position;
        diff.y = 0;

        float sqrDistance = diff.sqrMagnitude;
        float sqrRange = attackRange * attackRange;

        //Debug.Log($"거리 제곱: {sqrDistance} / 사거리 제곱: {sqrRange}");

        return sqrDistance < sqrRange;
    }

    public IEnumerator CoAttack(AttackData attackData, EntityController target, Team team)
    {
        if (attackData == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(attackData.firstAttackTime);

        if (target == null)
        {
            yield break;
        }

        CardArrangementManager.ReqeustAttack(attackData, transform.position, target, team);

        yield return new WaitForSeconds(attackData.lastDelay);
    }
}
