using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Gameendmanager gameendmanager;
    public ElixirManager elixir;
    public CardData selectedCard;
    public CardArrangementManager cardArrangementManager;
    public Team myTeam = Team.RedTeam;
    public HandManager handManager;
    public LayerMask Ground;
    public UnitHealthBarUI healthBarPrefab;
    public Transform worldUICanvas;

    public void UsedCard(int index, Vector2 screenPoint)
    {
        if (gameendmanager.Gameover)
        {
            return;
        }

        CardData card = handManager.handCards[index];

        if (card == null || card.cardDatas.Length == 0)
        {
            return;
        }
        EntityData entityData = card.cardDatas[0].entityData;

        if (elixir.currentElixir < entityData.elixir)
        {
            Debug.Log("엘릭서 부족");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, Ground))
        {
            Debug.Log("소환 가능한 바닥이 아님");
            return;
        }

        Vector3 spawnPos = hit.point;

        UseCard(index, card, entityData, spawnPos);
    }

    private void UseCard(int index, CardData card, EntityData entityData, Vector3 spawnPos)
    {

        cardArrangementManager.Arrangement(card, myTeam, spawnPos);

        elixir.currentElixir -= entityData.elixir;
        handManager.UseHandCard(index);
    }

    public void CreateHealthBar(UnitController unit)
    {
        if (unit == null)
        {
            return;
        }
        UnitHealthBarUI hpBar = Instantiate(healthBarPrefab, worldUICanvas);
        hpBar.Init(unit);
    }
}
