using System;
using System.Collections;
using UnityEngine;

public class CardArrangementManager : MonoBehaviour
{
    public static CardArrangementManager Instance;

    public static UnitController unit;
    public static TowerController tower;
    public static AttackEntityController spell;
    public UnitController unitPrefab;
    public TowerController towerPrefab;
    public AttackEntityController spellPrefab;

    public CardData KingTower;
    public CardData ArenaTower;
    public Transform RedTeamKing;
    public Transform BlueTeamKing;
    public Transform[] RedArena;
    public Transform[] BlueArena;
    public Transform Mid;
    public CardManager cardManager;

    public AI ai;

    private void Awake()
    {
        Instance = this;
    }

    private Vector3 GetKingTowerPosition(Team team) => team == Team.RedTeam ? RedTeamKing.position : BlueTeamKing.position;

    private void Start()
    {
        unit = unitPrefab;
        tower = towerPrefab;
        spell = spellPrefab;

        Arrangement(KingTower, Team.RedTeam, RedTeamKing.position);
        Arrangement(KingTower, Team.BlueTeam, BlueTeamKing.position);
        Arrangement(ArenaTower, Team.RedTeam, RedArena[0].position);
        Arrangement(ArenaTower, Team.RedTeam, RedArena[1].position);
        Arrangement(ArenaTower, Team.BlueTeam, BlueArena[0].position);
        Arrangement(ArenaTower, Team.BlueTeam, BlueArena[1].position);
    }

    private void LateUpdate()
    {
        EntityManager.isEntityUpdated = false;
    }

    public void Arrangement(CardData card, Team team, Vector3 point)
    {
        StartCoroutine(CoArrangement(card, team, point));
    }

    IEnumerator CoArrangement(CardData card, Team team, Vector3 point)
    {
        var entityDatas = card.cardDatas;
        float arrangementInterval = card.arrangmentCompletTime / entityDatas.Length;

        if (team == Team.BlueTeam && card.cardDatas[0].entityData.DefenseData != null
            && (card.cardDatas[0].entityData.DefenseData.entityType & EntityType.CrownTower) == 0)
        {
            ai.PlayerArrangementCard(card, point);
        }

        for (int i = 0; i < entityDatas.Length; i++)
        {
            GameObject entity = EntityArrangement(entityDatas[i].entityData);



            var controller = entity.GetComponent<RootController>();

            if (controller is EntityController e)
            {
                var adjust = entityDatas[i].positionAdjustment;

                if (team == Team.RedTeam)
                    adjust.z = -adjust.z;

                e.Init(entityDatas[i].entityData, point + adjust, team);

                if (controller != null)
                {
                    EntityManager.AddEntities(e);
                }

                UnitController realUnit = entity.GetComponent<UnitController>();

                if (realUnit != null)
                {
                    cardManager.CreateHealthBar(realUnit);
                }

               
            }
            else if (controller is AttackEntityController a)
            {
                var attack = entityDatas[i].entityData.AttackData;

                if (attack.toKingTower)
                {
                    ReqeustAttack(a, attack, GetKingTowerPosition(team), point, team);
                }
                else
                {
                    ReqeustAttack(a, attack, point, point, team);
                }
            }


            yield return new WaitForSeconds(arrangementInterval);
        }

        EntityManager.isEntityUpdated = true;
    }

    public static GameObject EntityArrangement(EntityData entityData)
    {
        if (entityData is UnitData u)
        {
            return Instantiate(unit.gameObject);
        }
        else if (entityData is TowerData t)
        {
            return Instantiate(tower.gameObject);
        }
        else if (entityData is SpellData s)
        {
            return Instantiate(spell.gameObject);
        }

        else return null;
    }

    public static void ReqeustAttack(AttackData attackData, Vector3 point, EntityController target, Team team)
    {
        var attack = Instantiate(spell);
        var attackComponent = attack.GetComponent<AttackEntityController>();
        attackComponent.Init(attackData, point, target, team);
    }
    public static void ReqeustAttack(AttackEntityController a, AttackData attackData, Vector3 point, Vector3 target, Team team)
    {
        var attackComponent = a.GetComponent<AttackEntityController>();
        attackComponent.Init(attackData, point, target, team);
    }
}
