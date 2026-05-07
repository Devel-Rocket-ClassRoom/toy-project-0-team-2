using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{

    public Queue<CardData> deckQueue = new Queue<CardData>();
    public List<CardData> deck = new List<CardData>();

    public CardData[] handCards = new CardData[4];
    public Button[] cardButtons = new Button[4];



    private void Start()
    {
        StartHand();
    }

    //처음덱만들기
    private void StartHand()
    {

        ShuffleDeck(deck);

        foreach (CardData card in deck)
        {
            deckQueue.Enqueue(card);
        }

        for (int i = 0; i < handCards.Length; i++)
        {
            handCards[i] = deckQueue.Dequeue();
        }
        CardData nextCard = deckQueue.Peek();
     
    }


    private void ShuffleDeck(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);

            CardData temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public void UseHandCard(int index)
    {
        CardData usedCard = handCards[index];

        deckQueue.Enqueue(usedCard);
        handCards[index] = deckQueue.Dequeue();
    }
}




