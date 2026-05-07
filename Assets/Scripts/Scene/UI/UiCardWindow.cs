using System.Collections.Generic;
using System.IO;
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

    private CardData[] deck;

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

        deck = new CardData[8];
    }

    protected override void OnShow()
    {
        OnRefreshCardList();
    }

    private void OnRefreshCardList()
    {
        foreach (Transform child in ScrollContent)
        {
            Destroy(child.gameObject);
        }

        HashSet<string> equippedCardNames = new HashSet<string>();
        foreach (var slot in CardSlots)
        {
            if (slot.childCount > 0)
            {
                equippedCardNames.Add(slot.GetChild(0).name.Replace("(Clone)", "").Trim());
            }
        }

        foreach (var card in CardPrefabs)
        {
            if (card == null)
            {
                continue;
            }

            if (equippedCardNames.Contains(card.cardName))
            {
                continue;
            }

            var newCard = Instantiate(card.cardImage, ScrollContent);
            if (newCard.GetComponent<UiCardPrefab>() == null)
            {
                newCard.AddComponent<UiCardPrefab>();
            }
        }
    }

    public void SetIntoDeck(CardData card, int idx)
    {
        deck[idx] = card;
    }
}