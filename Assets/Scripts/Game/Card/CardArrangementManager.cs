using System.Collections;
using UnityEngine;

public class CardArrangementManager: MonoBehaviour
{
    public UnitController unitPrefab;
    public TowerController towerPrefab;
    public SpellController spellPrefab;
    private float arrangmentCompletTime = 0.2f;

    public void Arrangement(CardData[] cardDatas, Team team, Vector3 point)
    {
        StartCoroutine(CoArrangement(cardDatas, team, point));
    }

    IEnumerator CoArrangement(CardData[] cards, Team team, Vector3 point)
    {
        float arrangementInterval = arrangmentCompletTime / cards.Length;

        for (int i = 0; i < cards.Length; i++)
        {
            GameObject entity = null;

            if (cards[i] is UnitData unit)
            {
                entity = Instantiate(unitPrefab.gameObject);
            }
            else if (cards[i] is TowerData tower)
            {
                entity = Instantiate(towerPrefab.gameObject);
            }
            else if (cards[i] is SpellData spell)
            {
                entity = Instantiate(spellPrefab.gameObject);
            }

            entity.transform.position = point + cards[i].positionAdjustment;

            var controller = entity.GetComponent<EntityController>();
            controller.Init(cards[i]);
            controller.team = team;

            EntityManager.AddEntities(controller);

            yield return new WaitForSeconds(arrangementInterval);
        }
    }
}
