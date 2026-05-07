using UnityEngine;
using UnityEngine.EventSystems;

public class UiCardSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // 드래그 중인 오브젝트가 카드라면 내 자식으로 편입
        if (eventData.pointerDrag != null)
        {
            // 만약 슬롯에 이미 카드가 하나만 있어야 한다면 여기서 체크 로직 추가
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.transform.localPosition = Vector3.zero;
        }
    }
}