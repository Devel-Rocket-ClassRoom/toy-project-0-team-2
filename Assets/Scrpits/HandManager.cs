using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{

    public Queue<Card> deckQueue = new Queue<Card>();
    public List<Card> deck = new List<Card>();

    public Card[] handCards = new Card[4];
    public Button[] cardButtons = new Button[4];



    private void Start()
    {
        StartHand();
    }

    //처음덱만들기
    private void StartHand()
    {

        ShuffleDeck(deck);

        foreach (Card card in deck)
        {
            deckQueue.Enqueue(card);
        }

        for (int i = 0; i < handCards.Length; i++)
        {
            handCards[i] = deckQueue.Dequeue();
        }
        Card nextCard = deckQueue.Peek();
     
    }


    private void ShuffleDeck(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);

            Card temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public void UseHandCard(int index)
    {
        Card usedCard = handCards[index];

        deckQueue.Enqueue(usedCard);
        handCards[index] = deckQueue.Dequeue();
    }
}




