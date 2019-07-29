using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIEventListener : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler
{
    public class InputEvent : UnityEvent<Vector2> { }

    public InputEvent onClick = new InputEvent();
    public InputEvent onDown = new InputEvent();
    public InputEvent onUp = new InputEvent();
    public InputEvent onDrag = new InputEvent();
    public InputEvent onDragEnd = new InputEvent();
    public InputEvent onDrop = new InputEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick == null)
            return;
        onClick.Invoke(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown == null)
            return;
        onDown.Invoke(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUp == null)
            return;
        onUp.Invoke(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag == null)
            return;
        onDrag.Invoke(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onDragEnd == null)
            return;
        onDragEnd.Invoke(eventData.position);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (onDrop == null)
            return;
        onDrag.Invoke(eventData.position);
    }
}
