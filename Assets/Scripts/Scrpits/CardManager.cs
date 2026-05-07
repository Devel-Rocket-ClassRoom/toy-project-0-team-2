using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Gameendmanager gameendmanager;
    public ElixirManager elixir;
    public CardData selectedCard;
    public CardArrangementManager cardArrangementManager;
    public Team myTeam = Team.RedTeam;
    public HandManager handManager;
    public UnitHealthBarUI healthBarPrefab;
    public Transform worldUICanvas;

    public LayerMask GroundLayerMask;
    public LayerMask redLayerMask;
    public LayerMask blueLayerMaskk;

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

        // 카드 타입 확인
        bool isSpell = entityData is SpellData;

        // 사용할 레이어 결정
        LayerMask currentMask;

        if (isSpell)
        {
            currentMask = GroundLayerMask;
        }
        else
        {
            currentMask = myTeam == Team.RedTeam? redLayerMask: blueLayerMaskk;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, currentMask))
        {
            Debug.Log("이 영역에는 소환 불가");
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

    public void CreateHealthBar(UnitController unit,Team team)
    {
        if (unit == null)
        {
            return;
        }
        UnitHealthBarUI hpBar = Instantiate(healthBarPrefab, worldUICanvas);
        hpBar.Init(unit, team);
    }
    public void CreateTowerHealthBar(TowerController tower, Team team)
    {
        if (tower == null)
        {
            return;
        }

        UnitHealthBarUI hpBar = Instantiate(healthBarPrefab, worldUICanvas);
        hpBar.InitTower(tower, team);
    }
}
