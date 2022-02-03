using UnityEngine;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private PlayerMovement _playermovement;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 direction = Vector2.zero; 
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
        {
            direction.x = eventData.delta.x / Mathf.Abs(eventData.delta.x);
        }
        else
        {
            direction.y = eventData.delta.y / Mathf.Abs(eventData.delta.y);
        }
        _playermovement.DirectionMove(direction);
        
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
