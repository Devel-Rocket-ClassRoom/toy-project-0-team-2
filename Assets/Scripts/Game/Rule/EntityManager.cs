using UnityEngine;
using System.Collections.Generic;
using System;

public static class EntityManager
{
    public static List<EntityController> redTeamEntities = new List<EntityController>();
    public static List<EntityController> blueTeamEntities = new List<EntityController>();

    public static List<TowerController> redTeamCrownTower = new List<TowerController>();
    public static List<TowerController> blueTeamCrownTower = new List<TowerController>();

    public static Action onEntitiesChanged;

    public static void AddEntities(EntityController entity)
    {
        if (entity.team == Team.RedTeam)
        {
            redTeamEntities.Add(entity);
        }
        else if (entity.team == Team.BlueTeam)
        {
            blueTeamEntities.Add(entity);
        }

        if (entity.cardData.DefenseData != null && (entity.cardData.DefenseData.entityType & EntityType.CrownTower) != 0)
        {
            if (entity.team == Team.RedTeam)
            {
                redTeamCrownTower.Add(entity as TowerController);
            }
            else if (entity.team == Team.BlueTeam)
            {
                blueTeamCrownTower.Add(entity as TowerController);
            }
        }

        onEntitiesChanged?.Invoke();
    }

    public static void RemoveEntities(EntityController entity)
    {
        if (entity.team == Team.RedTeam)
        {
            redTeamEntities.Remove(entity);
        }
        else if (entity.team == Team.BlueTeam)
        {
            blueTeamEntities.Remove(entity);
        }

        if (entity.cardData.DefenseData != null && entity.cardData.DefenseData.entityType == EntityType.CrownTower)
        {
            if (entity.team == Team.RedTeam)
            {
                redTeamCrownTower.Remove(entity as TowerController);
            }
            else if (entity.team == Team.BlueTeam)
            {
                blueTeamCrownTower.Remove(entity as TowerController);
            }
        }

        onEntitiesChanged?.Invoke();
    }
}
