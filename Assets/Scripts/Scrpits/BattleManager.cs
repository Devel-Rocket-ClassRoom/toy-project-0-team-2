using System.Collections;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //게임 엔드
    public bool Gameover;
    //엘리서 정보
    private const float StartElixir = 6;
    private const float maxElixir = 10;
    private const float regenRate = 0.35f;
    public float currentElixir;

    //타이머
    public int battleTime = 180;
    //카드 데이터
    public CardData selectedCard;
    public CardArrangementManager cardArrangementManager;
    public Team myTeam = Team.RedTeam;
    private Vector3 spawnPoint;
    public HandManager handManager;

    public LayerMask Ground;


    void Start()
    {
        StartCoroutine(BattleTimer());
        currentElixir = StartElixir;
    }

    void Update()
    {
        if (!Gameover)
        {
            currentElixir += Time.deltaTime * regenRate;
            currentElixir = Mathf.Clamp(currentElixir, 0, maxElixir);
        }

    }

   //타이머
    IEnumerator BattleTimer()
    {
        while (battleTime > 0&& !Gameover)
        {
            yield return new WaitForSeconds(1f);
            battleTime--;
            if (battleTime < 0)
            {
                Gameover=true;
            }
        }
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
        EntityData cardData = card.cardDatas[0].entityData;

        if (currentElixir < cardData.elixir)
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

        currentElixir -= cardData.elixir;

        cardArrangementManager.Arrangement(card, myTeam, spawnPos);

        handManager.UseHandCard(index);

    }


    //버튼
    //public void OnUsedCard(int index)
    //{
    //    if (Gameover)
    //    {
    //        return;
    //    }
    //    Card card = handManager.handCards[index];

    //    if (card == null || card.cardDatas.Length == 0)
    //    {
    //        return;
    //    }
    //    CardData cardData = card.cardDatas[0];
    //    int cost = cardData.elixir;

    //    if (currentElixir < cost)
    //    {
    //        Debug.Log("엘릭서 부족");
    //        return;
    //    }

    //    currentElixir -= cost;

    //    cardArrangementManager.Arrangement( new CardData[] { cardData }, myTeam,spawnPoint);

    //    handManager.UseHandCard(index);
    //}


}