using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public timerManager timerManager;
    public CardManager cardManager;

    public int index;

    public GameObject worldPreviewPrefab;
    private GameObject previewObj;

    public LayerMask groundMask;

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        previewObj = Instantiate(worldPreviewPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        MovePreview(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        cardManager.UsedCard(index, eventData.position);

        if (previewObj != null)
        {
            Destroy(previewObj);
        }
    }

    void MovePreview(PointerEventData eventData)
    {
        if (previewObj == null)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            previewObj.transform.position = hit.point + Vector3.up * 0.5f;
        }
    }
}