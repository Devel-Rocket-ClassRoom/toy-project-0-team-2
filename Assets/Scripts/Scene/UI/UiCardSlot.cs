using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiCardSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (transform.childCount > 0)
            {
                Transform existingCard = transform.GetChild(0);

                var cardWindow = UiManager.Instance.CardWindow;
                if (cardWindow != null && cardWindow.ScrollContent != null)
                {
                    existingCard.SetParent(cardWindow.ScrollContent);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(cardWindow.ScrollContent.GetComponent<RectTransform>());
                }
            }

            GameObject newCard = eventData.pointerDrag;
            newCard.transform.SetParent(this.transform);
            newCard.transform.localPosition = Vector3.zero;
        }
    }
}