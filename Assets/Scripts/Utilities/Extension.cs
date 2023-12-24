using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        return Utilities.GetOrAddComponent<T>(obj);
    }

    public static void BindEvent<T>(this GameObject obj, Action<T> action = null, EventTriggerType eventType = EventTriggerType.PointerClick, PointerEventData.InputButton? inputButton = PointerEventData.InputButton.Left) where T : BaseEventData
    {
        UI_Base.BindEvent(obj, action, eventType, inputButton);
    }

    public static bool IsValid(this GameObject obj)
    {
        return obj != null && obj.activeSelf;
    }

    //public static bool IsValid(this Thing thing)
    //{
    //    return thing != null && thing.isActiveAndEnabled;
    //}
}