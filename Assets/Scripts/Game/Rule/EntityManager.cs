using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

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
            if (entity.team == Team.RedTeam)
            {
                redTeamCrownTower.Remove(entity);
            }
            else if (entity.team == Team.BlueTeam)
            {
                blueTeamCrownTower.Remove(entity);
            }

            if (entity.cardData.cardName == "King")
            {
                int count = 0;

                if (entity.team == Team.RedTeam)
                {
                    count = redTeamCrownTower.Count;

                    for (int i = count - 1; i >= 0; i--)
                    {
                        redTeamCrownTower[i].Dead();
                        onCrounTowerDestroy?.Invoke(entity.team);
                    }
                }
                else
                {
                    count = blueTeamCrownTower.Count;

                    for (int i = count - 1; i >= 0; i--)
                    {
                        blueTeamCrownTower[i].Dead();
                        onCrounTowerDestroy?.Invoke(entity.team);
                    }
                }

                return;
            }

            onCrounTowerDestroy?.Invoke(entity.team);
        }

        onEntitiesChanged?.Invoke();
    }
}
