using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCardWindow : UiBaseWindow
{
    [Header("Player Display")]
    public TMP_Text PlayerNameText;
    public Image PlayerColorBar;

    [Header("Card Deck")]
    public List<CardData> CardPrefabs;
    public Transform ScrollContent;
    public Transform[] CardSlots;

    private bool _deckLoaded = false;

    protected override void Awake()
    {
        base.Awake();

        foreach (var slot in CardSlots)
        {
            if (slot.GetComponent<UiCardSlot>() == null)
            {
                slot.gameObject.AddComponent<UiCardSlot>();
            }
        }
    }

    protected override void OnShow()
    {
        if (!_deckLoaded)
        {
            _deckLoaded = true;
            LoadSavedDeck();
        }
        OnRefreshCardList();
    }

    private void LoadSavedDeck()
    {
        DeckContainer.Instance.Deck = new CardData[8];

        string[] savedNames = DeckContainer.Instance.LoadDeckNames();
        if (savedNames == null)
            return;

        for (int i = 0; i < CardSlots.Length && i < savedNames.Length; i++)
        {
            if (string.IsNullOrEmpty(savedNames[i]))
                continue;

            CardData card = CardPrefabs.Find(c => c != null && c.cardName == savedNames[i]);
            if (card == null)
                continue;

            var newCard = Instantiate(card.cardImage, CardSlots[i]);
            newCard.transform.localPosition = Vector3.zero;
            var prefabScript = newCard.GetComponent<UiCardPrefab>();
            if (prefabScript == null)
                prefabScript = newCard.AddComponent<UiCardPrefab>();
            prefabScript.cardData = card;
        }
    }

    private void OnRefreshCardList()
    {
        foreach (Transform child in ScrollContent)
        {
            Destroy(child.gameObject);
        }

        HashSet<CardData> equippedCards = new HashSet<CardData>();
        foreach (var slot in CardSlots)
        {
            if (slot.childCount == 0)
                continue;

            var prefabScript = slot.GetChild(0).GetComponent<UiCardPrefab>();
            if (prefabScript != null && prefabScript.cardData != null)
            {
                equippedCards.Add(prefabScript.cardData);
            }
        }

        foreach (var card in CardPrefabs)
        {
            if (card == null)
            {
                continue;
            }

            if (equippedCards.Contains(card))
            {
                continue;
            }

            var newCard = Instantiate(card.cardImage, ScrollContent);
            var prefabScript = newCard.GetComponent<UiCardPrefab>();
            if (prefabScript == null)
            {
                prefabScript = newCard.AddComponent<UiCardPrefab>();
            }
            prefabScript.cardData = card;
        }
    }

    public void SetIntoDeck()
    {
        DeckContainer.Instance.Deck = GetEquippedCardDatas();

        int count = 0;
        foreach (var data in DeckContainer.Instance.Deck)
        {
            if (data != null)
            {
                count++;
            }
        }

        if (count < 8)
        {
            Debug.LogWarning($"덱이 미완성입니다. (현재: {count}/8)");
            return;
        }
    }

    public void OnClickSaveDeck()
    {
        DeckContainer.Instance.SaveDeck(GetEquippedCardDatas());
        Debug.Log("덱이 저장되었습니다.");
    }

    public CardData[] GetEquippedCardDatas()
    {
        CardData[] currentDeck = new CardData[8];

        for (int i = 0; i < CardSlots.Length; i++)
        {
            if (i >= 8)
            {
                break;
            }

            if (CardSlots[i].childCount > 0)
            {
                var cardPrefab = CardSlots[i].GetChild(0).GetComponent<UiCardPrefab>();
                if (cardPrefab != null)
                {
                    currentDeck[i] = cardPrefab.cardData;
                }
            }
        }
        return currentDeck;
    }
}
