using UnityEngine;
using System.Collections.Generic;
using System;

public static class EntityManager
{
    public static bool isEntityUpdated = false;

    public static List<EntityController> redTeamEntities = new List<EntityController>();
    public static List<EntityController> blueTeamEntities = new List<EntityController>();

    public static List<EntityController> redTeamCrownTower = new List<EntityController>();
    public static List<EntityController> blueTeamCrownTower = new List<EntityController>();

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
                redTeamCrownTower.Add(entity);
            }
            else if (entity.team == Team.BlueTeam)
            {
                blueTeamCrownTower.Add(entity);
            }
        }

        isEntityUpdated = true;
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
                redTeamCrownTower.Remove(entity);
            }
            else if (entity.team == Team.BlueTeam)
            {
                blueTeamCrownTower.Remove(entity);
            }
        }

        onEntitiesChanged?.Invoke();
    }
}
