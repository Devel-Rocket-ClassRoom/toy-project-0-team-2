using UnityEngine;
using UnityEngine.EventSystems;

public class UiCardSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (transform.childCount > 0)
            {
                var existingCard = transform.GetChild(0);
                var window = GetComponentInParent<UiCardWindow>();
                if (window != null)
                {
                    existingCard.SetParent(window.ScrollContent);
                }
            }

            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.transform.localPosition = Vector3.zero;
        }
    }
}