using UnityEngine;
using System.Collections.Generic;
using System;

public static class EntityManager
{
    public static List<EntityController> redTeamEntities = new List<EntityController>();
    public static List<EntityController> blueTeamEnntities = new List<EntityController>();

    public static List<TowerController> redTeamCrownTower = new List<TowerController>();
    public static List<TowerController> blueTeamCrownTower = new List<TowerController>();

    public static Action onEntitiesChanged;

    public static void AddEntities(EntityController entity)
    {
        if (entity.team == Team.RedTeam)
        {
            redTeamEntities.Add(entity);
            Debug.Log(redTeamEntities.Count);
        }
        else if (entity.team == Team.BlueTeam)
        {
            blueTeamEnntities.Add(entity);
            Debug.Log(blueTeamEnntities.Count);
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
            blueTeamEnntities.Remove(entity);
        }

        onEntitiesChanged?.Invoke();
    }
}
