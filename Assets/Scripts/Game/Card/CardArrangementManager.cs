using System.Collections;
using UnityEngine;

public class CardArrangementManager: MonoBehaviour
{
    public static UnitController unit;
    public static TowerController tower;
    public static AttackEntityController spell;
    public UnitController unitPrefab;
    public TowerController towerPrefab;
    public AttackEntityController spellPrefab;
    public CardData KingTower;
    public Transform RedTeamKing;
    public Transform BlueTeamKing;

    private Vector3 GetKingTowerPosition(Team team) => team == Team.RedTeam ? RedTeamKing.position : BlueTeamKing.position;

    private void Start()
    {
        unit = unitPrefab;
        tower = towerPrefab;
        spell = spellPrefab;

        Arrangement(KingTower, Team.RedTeam, RedTeamKing.position);
        Arrangement(KingTower, Team.BlueTeam, BlueTeamKing.position);
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

        for (int i = 0; i < entityDatas.Length; i++)
        {
            GameObject entity = EntityArrangement(entityDatas[i].entityData);

            var controller = entity.GetComponent<RootController>();
            
            if (controller is EntityController e)
            {
                e.Init(entityDatas[i].entityData, point + entityDatas[i].positionAdjustment, team);

                if (controller != null)
                {
                    EntityManager.AddEntities(e);
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
