using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiCardPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private ScrollRect parentScrollRect;

    public CardData cardData;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        parentScrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        startPosition = transform.position;

        if (parentScrollRect == null)
        {
            parentScrollRect = GetComponentInParent<ScrollRect>();
        }

        if (parentScrollRect != null)
        {
            parentScrollRect.StopMovement();
            parentScrollRect.enabled = false;
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;

        transform.SetParent(UiManager.Instance.CardWindow.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (parentScrollRect != null)
        {
            parentScrollRect.enabled = true;
        }

        bool isStuckInSlot = GetComponentInParent<UiCardSlot>() != null;
        if (!isStuckInSlot)
        {
            var cardWindow = UiManager.Instance.CardWindow;
            if (cardWindow != null && cardWindow.ScrollContent != null)
            {
                transform.SetParent(cardWindow.ScrollContent);
                LayoutRebuilder.ForceRebuildLayoutImmediate(cardWindow.ScrollContent.GetComponent<RectTransform>());
            }
            else
            {
                transform.SetParent(originalParent);
                transform.position = startPosition;
            }
        }
    }
}