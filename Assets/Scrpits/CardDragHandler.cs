using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public BattleManager battleManager;

    public int index;

    public GameObject worldPreviewPrefab;
    private GameObject previewObj;

    public LayerMask groundMask;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (battleManager.Gameover)
        { 
          return;
        }
        previewObj = Instantiate(worldPreviewPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (battleManager.Gameover)
        {
            return;
        }
        MovePreview(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (battleManager.Gameover)
        {
            return;
        }
        battleManager.UsedCard(index, eventData.position);

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