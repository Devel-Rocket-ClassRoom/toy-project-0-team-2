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
    public List<GameObject> CardPrefabs;
    public Transform ScrollContent;
    public Transform[] CardSlots;

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

        foreach (var prefab in CardPrefabs)
        {
            if (prefab == null)
            {
                continue;
            }

            if (equippedCardNames.Contains(prefab.name))
            {
                continue;
            }

            var newCard = Instantiate(prefab, ScrollContent);
            if (newCard.GetComponent<UiCardPrefab>() == null)
            {
                newCard.AddComponent<UiCardPrefab>();
            }
        }
    }
}