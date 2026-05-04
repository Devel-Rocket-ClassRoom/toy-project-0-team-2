using NUnit.Framework.Internal;
using System.Drawing;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{

    public EntityController FindNearestTarget(Team team, EntityType attackFilter, float sightRange)
    {
        if (sightRange <= 0) return null;

        var entities = team == Team.RedTeam ? EntityManager.blueTeamEntities : EntityManager.redTeamEntities;

        EntityController result = null;
        float min = float.MaxValue;

        foreach (var entity in entities)
        {
            if ((attackFilter & entity.entityType) == 0) { continue; }
            if (entity == this) { continue; }

            float range = sightRange + entity.size;
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

        return result;
    }

    public TowerController FindNearestCrownTower(Team team, EntityType attackFilter)
    {
        var towers = team == Team.RedTeam ? EntityManager.blueTeamCrownTower : EntityManager.redTeamCrownTower;
        if (towers.Count == 0) return null;

        TowerController result = null;
        float min = float.MaxValue;

        foreach (TowerController tower in towers)
        {
            if (tower == this || tower == null) continue;
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
