using UnityEngine;
using UnityEngine.EventSystems;

public class UiCardPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        startPosition = transform.position;

        // 드래그하는 동안 다른 UI 요소를 통과해서 감지될 수 있도록 설정
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;

        // 드래그 시 최상단에 보이도록 부모를 잠시 바꿈 (UI 레이어 순서 때문)
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 위치를 따라다니게 함
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // 마우스 아래에 'DropZone'이 없다면 원래 위치로 복귀
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent);
            transform.position = startPosition;
        }
    }
}