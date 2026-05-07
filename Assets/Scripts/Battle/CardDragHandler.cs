using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardManager cardManager; // 인스펙터에서 연결
    public int index;

    private GameObject previewObj;

    public Renderer blueAreaRenderer;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.blue;
    public LayerMask groundMask;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (cardManager.gameendmanager.Gameover)
        {
            return;
        }

        CardData card = cardManager.handManager.handCards[index];

        if (card == null || card.cardDatas.Length == 0)
        {
            return;
        }

        EntityData entityData = card.cardDatas[0].entityData;

        if (entityData.previewmodel == null)
        {
            Debug.LogWarning($"{entityData.cardName} previewmodel이 없음");
            return;
        }

        previewObj = Instantiate(entityData.previewmodel);

        if (blueAreaRenderer != null)
        {
            blueAreaRenderer.material.color = highlightColor;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        MovePreview(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cardManager.gameendmanager.Gameover)
        {
            return;
        }

        cardManager.UsedCard(index, eventData.position);

        if (previewObj != null)
        {
            Destroy(previewObj);
        }

        if (blueAreaRenderer != null)
        {
            blueAreaRenderer.material.color = normalColor;
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