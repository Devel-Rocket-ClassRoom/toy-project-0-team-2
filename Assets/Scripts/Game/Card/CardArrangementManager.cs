using System.Collections;
using UnityEngine;

public class CardArrangementManager: MonoBehaviour
{
    public static UnitController unit;
    public static TowerController tower;
    public static SpellController spell;
    public UnitController unitPrefab;
    public TowerController towerPrefab;
    public SpellController spellPrefab;
    public Card KingTower;
    public Transform RedTeamKing;
    public Transform BlueTeamKing;

    private void Start()
    {
        unit = unitPrefab;
        tower = towerPrefab;
        spell = spellPrefab;

        Arrangement(KingTower, Team.RedTeam, RedTeamKing.position);
        Arrangement(KingTower, Team.BlueTeam, BlueTeamKing.position);
    }

    public void Arrangement(Card card, Team team, Vector3 point)
    {
        StartCoroutine(CoArrangement(card, team, point));
    }

    IEnumerator CoArrangement(Card card, Team team, Vector3 point)
    {
        var cardDatas = card.cardDatas;
        float arrangementInterval = card.arrangmentCompletTime / cardDatas.Length;

        for (int i = 0; i < cardDatas.Length; i++)
        {
            GameObject entity = null;

            if (cardDatas[i].cardData is UnitData unit)
            {
                entity = Instantiate(unitPrefab.gameObject);
            }
            else if (cardDatas[i].cardData is TowerData tower)
            {
                entity = Instantiate(towerPrefab.gameObject);
            }
            else if (cardDatas[i].cardData is SpellData spell)
            {
                entity = Instantiate(spellPrefab.gameObject);
            }

            var controller = entity.GetComponent<EntityController>();
            controller.team = team;
            controller.Init(cardDatas[i].cardData, point);

            if (controller is not SpellController)
            {
                EntityManager.AddEntities(controller);
            }

            yield return new WaitForSeconds(arrangementInterval);
        }
    }
}
