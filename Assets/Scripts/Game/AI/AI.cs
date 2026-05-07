using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float currentElixer;
    public CardData[] originDeck;
    private Queue<CardData> deck;
    private CardData[] hand = new CardData[4];
    public CardArrangementManager arrangementManager;
    private Team team = Team.RedTeam;

    public bool isActive;

    private void Start()
    {
        deck = new Queue<CardData>();

        for (int i = 0; i < originDeck.Length; i++)
        {
            deck.Enqueue(originDeck[i]);
        }

        for (int i = 0; i < hand.Length; i++)
        {
            hand[i] = deck.Dequeue();
            deck.Enqueue(hand[i]);
        }
    }

    public void PlayerArrangementCard(CardData card, Vector3 point)
    {
        Debug.Log(point);
        if (!isActive) return;

        Debug.Log($"{card.name} : {card.elixer}, {card.cardDatas.Length}");

        var cardType = ClassifyCard(card);
        Debug.Log(cardType);

        var selectedCard = SelectCard(cardType);
        Debug.Log(selectedCard);

        if (selectedCard != -1)
        {
            var method = ChooseReactMethod(hand[selectedCard], card, point);
            Debug.Log(method);

            ArrangementCard(hand[selectedCard], card, point, method);
            hand[selectedCard] = deck.Dequeue();
            deck.Enqueue(hand[selectedCard]);
        }
        else
        {
            Debug.Log("AI는 카드를 내지 않았다");
        }
    }

    private EntityTypeDetail ClassifyCard(CardData card)
    {
        if (card.cardDatas[0].entityData.DefenseData != null)
        {
            if (card.elixer >= 6
            && card.cardDatas.Length == 1
            && (card.cardDatas[0].entityData.DefenseData.entityType & (EntityType.Aerial | EntityType.Ground)) != 0)
            {
                return EntityTypeDetail.BigUnit;
            }

            if (card.elixer >= 3 && card.elixer <= 6
                && card.cardDatas.Length <= 6
                && (card.cardDatas[0].entityData.DefenseData.entityType & (EntityType.Aerial | EntityType.Ground)) != 0)
            {
                return EntityTypeDetail.MiddleUnit;
            }

            if (card.elixer >= 2 && card.elixer <= 4
                && card.cardDatas.Length >= 3
                && (card.cardDatas[0].entityData.DefenseData.entityType & (EntityType.Aerial | EntityType.Ground)) != 0)
            {
                return EntityTypeDetail.WiniUnit;
            }

            if (card.elixer == 1)
            {
                return EntityTypeDetail.Recycle;
            }

            if ((card.cardDatas[0].entityData.DefenseData.entityType & EntityType.Tower) != 0)
            {
                return EntityTypeDetail.Tower;
            }
        }

        return EntityTypeDetail.Magic;
    }

    private int SelectCard(EntityTypeDetail cardType)
    {
        int card = -1;

        switch (cardType)
        {
            case EntityTypeDetail.BigUnit:
                card = CheckHand(EntityTypeDetail.BigUnit);
                if (card == -1) card = CheckHand(EntityTypeDetail.WiniUnit);
                if (card == -1) card = CheckHand(EntityTypeDetail.Tower);
                if (card == -1) card = CheckHand(EntityTypeDetail.Recycle);
                break;

            case EntityTypeDetail.MiddleUnit:
                card = CheckHand(EntityTypeDetail.BigUnit);
                if (card == -1) card = CheckHand(EntityTypeDetail.MiddleUnit);
                if (card == -1) card = CheckHand(EntityTypeDetail.Magic);
                if (card == -1) card = CheckHand(EntityTypeDetail.Tower);
                if (card == -1) card = CheckHand(EntityTypeDetail.Recycle);
                break;

            case EntityTypeDetail.WiniUnit:
                card = CheckHand(EntityTypeDetail.MiddleUnit);
                if (card == -1) card = CheckHand(EntityTypeDetail.Magic);
                if (card == -1) card = CheckHand(EntityTypeDetail.WiniUnit);
                if (card == -1) card = CheckHand(EntityTypeDetail.Recycle);
                break;
        }

        return card;
    }

    private int CheckHand(EntityTypeDetail type)
    {
        if (type == EntityTypeDetail.Recycle)
        {
            var selected = 0;

            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i].elixer < hand[selected].elixer)
                {
                    selected = i;
                }
            }

            return selected;
        }

        for (int i = 0; i < hand.Length; i++)
        {
            if (hand[i].elixer <= currentElixer && ClassifyCard(hand[i]) == type)
            {
                return i;
            }
        }


        return -1;
    }

    private ReactMethod ChooseReactMethod(CardData card, CardData enemyCard, Vector3 point)
    {
        if (card.cardDatas[0].entityData is SpellData)
        {
            return ReactMethod.Magic;
        }

        if (enemyCard.cardDatas[0].entityData is UnitData u)
        {
            if (u.tilePerSeconds >= 1.5 || (u.SpecialData != null && (u.SpecialData.hasSprint || u.SpecialData.hasCharge)))
            {
                if (point.z < EntityMover.HorizontalMidLine - EntityMover.ArenaTowerLine)
                {
                    return ReactMethod.ArenaTowerShiled;
                }
                else if (point.z < EntityMover.HorizontalMidLine)
                {
                    return ReactMethod.DefenseArenaTower;
                }
                else
                {
                    return ReactMethod.Mid;
                }
            }

            else
            {
                if (point.z < EntityMover.HorizontalMidLine - EntityMover.ArenaTowerLine)
                {
                    return ReactMethod.Rear;
                }
                else if (point.z < EntityMover.HorizontalMidLine)
                {
                    return ReactMethod.ArenaTowerShiled;
                }
                else
                {
                    return ReactMethod.DefenseArenaTower;
                }
            }
        }

        return ReactMethod.None;
    }

    private void ArrangementCard(CardData card, CardData enemyCard, Vector3 point, ReactMethod method)
    {
        int reverse = point.x > EntityMover.VerticalMidLine ? 1 : -1;

        switch (method)
        {
            case ReactMethod.AfterAcrossBridge:
                StartCoroutine(CoArrangementBridge(card, enemyCard, point));
                break;
            case ReactMethod.ArenaTowerShiled:
                arrangementManager.Arrangement(card, team, 
                    new Vector3(EntityMover.VerticalMidLine + (EntityMover.RoadLine - 2) * reverse, 0,
                    EntityMover.HorizontalMidLine + EntityMover.ArenaTowerLine + 2));
                break;
            case ReactMethod.DefenseArenaTower:
                arrangementManager.Arrangement(card, team,
                    new Vector3(EntityMover.VerticalMidLine + (EntityMover.RoadLine) * reverse, 0,
                    EntityMover.HorizontalMidLine + EntityMover.ArenaTowerLine - 2));
                break;
            case ReactMethod.Rear:
                int rand1 = Random.Range(0, 2);
                if (rand1 == 0) arrangementManager.Arrangement(card, team,
                    new Vector3(EntityMover.VerticalMidLine + (8.5f) * reverse, 0,
                    EntityMover.HorizontalMidLine + 14.5f));
                else if (rand1 == 1) arrangementManager.Arrangement(card, team,
                    new Vector3(EntityMover.VerticalMidLine + (2.5f) * reverse, 0,
                    EntityMover.HorizontalMidLine + 15.5f));
                break;
            case ReactMethod.Mid:
                arrangementManager.Arrangement(card, team,
                    new Vector3(EntityMover.VerticalMidLine - 0.5f, 0,
                    EntityMover.HorizontalMidLine + 4.5f) + new Vector3(Random.Range(0, 2), 0, Random.Range(0, 2)));
                break;
            case ReactMethod.Magic:
                StartCoroutine(CoArrangementMagic(card, enemyCard, point));
                break;
        }
    }

    IEnumerator CoArrangementBridge(CardData card, CardData enemyCard, Vector3 point)
    {
        int reverse = point.x > EntityMover.VerticalMidLine ? 1 : -1;
        float speed = (enemyCard.cardDatas[0].entityData as UnitData).tilePerSeconds;
        yield return new WaitForSeconds(EntityMover.HorizontalMidLine - point.z / speed + enemyCard.arrangmentCompletTime + 0.5f);
        arrangementManager.Arrangement(card, team,
                    new Vector3(EntityMover.VerticalMidLine + EntityMover.RoadLine * reverse, 0,
                    EntityMover.HorizontalMidLine - 1.5f));
    }

    IEnumerator CoArrangementMagic(CardData card, CardData enemyCard, Vector3 point)
    {
        if (card.cardDatas[0].entityData is SpellData s)
        {
            float radius = s.AttackData.attackRadius;

            if (point.z > EntityMover.HorizontalMidLine - EntityMover.ArenaTowerLine)
            {
                int reverse = point.x > EntityMover.VerticalMidLine ? 1 : -1;
                float speed = (enemyCard.cardDatas[0].entityData as UnitData).tilePerSeconds;
                yield return new WaitForSeconds(EntityMover.ArenaTowerLine - point.z / speed + enemyCard.arrangmentCompletTime);
                arrangementManager.Arrangement(card, team,
                            new Vector3(EntityMover.VerticalMidLine + EntityMover.RoadLine * reverse, 0,
                            EntityMover.ArenaTowerLine + 1 + radius));
            }
            else
            {
                yield return new WaitForSeconds(card.arrangmentCompletTime);
                arrangementManager.Arrangement(card, team,
                        point + new Vector3(0, 0, radius));
            }
        }
    }
}

public enum EntityTypeDetail
{
    None,
    Recycle,
    BigUnit,
    MiddleUnit,
    WiniUnit,
    Tower,
    Magic,
}

public enum ReactMethod
{
    None,
    AfterAcrossBridge,
    ArenaTowerShiled,
    DefenseArenaTower,
    Rear,
    Mid,
    Magic,
}
