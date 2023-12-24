using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler,
        IInitializePotentialDragHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IScrollHandler,
        IUpdateSelectedHandler,
        ISelectHandler,
        IDeselectHandler,
        IMoveHandler,
        ISubmitHandler,
        ICancelHandler
{
    public class Entry
    {
        public EventTriggerType eventID = EventTriggerType.PointerClick;
        public Action<BaseEventData> callback;
        public PointerEventData.InputButton? inputButton = PointerEventData.InputButton.Left;
    }

    private List<Entry> _delegates;

    public List<Entry> Triggers
    {
        get
        {
            if (_delegates == null)
                _delegates = new List<Entry>();
            return _delegates;
        }
        set { _delegates = value; }
    }

    private void Execute<T>(EventTriggerType id, T eventData) where T : BaseEventData
    {
        for (int i = 0; i < Triggers.Count; ++i)
        {
            var ent = Triggers[i];
            if (ent.eventID == id)
            {
                if (eventData is PointerEventData pointer && (pointer.button == ent.inputButton))
                    ent.callback?.Invoke(eventData);
                else
                    ent.callback?.Invoke(eventData);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Execute(EventTriggerType.PointerEnter, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Execute(EventTriggerType.PointerExit, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Execute(EventTriggerType.Drag, eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Execute(EventTriggerType.Drop, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Execute(EventTriggerType.PointerDown, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Execute(EventTriggerType.PointerUp, eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Execute(EventTriggerType.PointerClick, eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Execute(EventTriggerType.Select, eventData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Execute(EventTriggerType.Deselect, eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        Execute(EventTriggerType.Scroll, eventData);
    }

    public void OnMove(AxisEventData eventData)
    {
        Execute(EventTriggerType.Move, eventData);
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        Execute(EventTriggerType.UpdateSelected, eventData);
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        Execute(EventTriggerType.InitializePotentialDrag, eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Execute(EventTriggerType.BeginDrag, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Execute(EventTriggerType.EndDrag, eventData);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Execute(EventTriggerType.Submit, eventData);
    }

    public void OnCancel(BaseEventData eventData)
    {
        Execute(EventTriggerType.Cancel, eventData);
    }
}