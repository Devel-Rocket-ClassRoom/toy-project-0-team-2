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
    public static event Action<Team> onCrounTowerDestroy;


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

        if (entity.cardData.DefenseData != null && (entity.cardData.DefenseData.entityType & EntityType.CrownTower) != 0)
        {
            if (entity.cardData.cardName == "King Tower")
            {
                Debug.Log(0);

                if (entity.team == Team.RedTeam)
                {
                    for (int i = 0; i < redTeamCrownTower.Count; i++)
                    {
                        onCrounTowerDestroy?.Invoke(entity.team);
                    }

                    redTeamCrownTower.Clear();
                }
                else
                {
                    for (int i = 0; i < blueTeamCrownTower.Count; i++)
                    {
                        onCrounTowerDestroy?.Invoke(entity.team);
                    }

                    blueTeamCrownTower.Clear();
                }

                return;
            }

            if (entity.team == Team.RedTeam)
            {
                redTeamCrownTower.Remove(entity);
            }
            else if (entity.team == Team.BlueTeam)
            {
                blueTeamCrownTower.Remove(entity);
            }

            onCrounTowerDestroy?.Invoke(entity.team);
        }

        onEntitiesChanged?.Invoke();
    }
}
