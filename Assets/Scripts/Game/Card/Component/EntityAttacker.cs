using UnityEngine;
using System.Collections;

public class EntityAttacker : MonoBehaviour
{
    public bool IsTargetInRange(EntityController target, float attackRange, out float sqrDistance)
    {
        if (target == null) { sqrDistance = 0; return false; }
        bool result = IsTargetInRange(target.transform.position, attackRange + target.size, out sqrDistance);
        return result;
    }

    public bool IsTargetInRange(Vector3 target, float attackRange, out float sqrDistance)
    {
        if (attackRange <= 0) { sqrDistance = 0; return false; }

        Vector3 diff = target - transform.position;
        diff.y = 0;

        sqrDistance = diff.sqrMagnitude;
        float sqrRange = attackRange * attackRange;

        //Debug.Log($"거리 제곱: {sqrDistance} / 사거리 제곱: {sqrRange}");
        
        return sqrDistance < sqrRange;
    }

    public IEnumerator CoAttack(AttackData attackData, Vector3 position, EntityController target, Team team)
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

        CardArrangementManager.ReqeustAttack(attackData, position, target, team);

        yield return new WaitForSeconds(attackData.lastDelay);
    }
}
