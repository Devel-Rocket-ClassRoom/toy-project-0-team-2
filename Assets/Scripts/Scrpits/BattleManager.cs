using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool Gameover;

    private const float StartElixir = 6;
    private const float maxElixir = 10;
    private const float regenRate = 0.35f;

    public float currentElixir;
    public int battleTime = 180;

    public CardData selectedCard;
    public CardArrangementManager cardArrangementManager;
    public Team myTeam = Team.RedTeam;
    public HandManager handManager;

    public LayerMask Ground;

    public UnitHealthBarUI healthBarPrefab;
    public Transform worldUICanvas;

    private void Start()
    {
        currentElixir = StartElixir;
        StartCoroutine(BattleTimer());
    }

    private void Update()
    {
        if (Gameover)
        {
            return;
        }

        currentElixir += Time.deltaTime * regenRate;
        currentElixir = Mathf.Clamp(currentElixir, 0, maxElixir);
    }

    private IEnumerator BattleTimer()
    {
        while (battleTime > 0 && !Gameover)
        {
            yield return new WaitForSeconds(1f);
            battleTime--;
        }

        Gameover = true;
    }

    public void UsedCard(int index, Vector2 screenPoint)
    {
        if (Gameover)
        {
            return;
        }

        CardData card = handManager.handCards[index];

        if (card == null || card.cardDatas.Length == 0)
        {
            return;
        }
        EntityData entityData = card.cardDatas[0].entityData;

        if (currentElixir < entityData.elixir)
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

        currentElixir -= entityData.elixir;
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