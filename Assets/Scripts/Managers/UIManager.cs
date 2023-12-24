using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private GameObject _root;
    public GameObject Root
    {
        get
        {
            _root = GameObject.Find("@UI_Root");
            if (_root == null)
                _root = new("@UI_Root");
            return _root;
        }
    }

    private UI_Scene _sceneUI;
    public UI_Scene SceneUI => _sceneUI;

    private int _popUpOrder = 10;
    private Stack<UI_PopUp> _popUpStack = new();
    public int PopUpCount => _popUpStack.Count;

    public void SetCanvas(GameObject obj, int? sortOrder = 0)
    {
        Canvas canvas = obj.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        CanvasScaler scaler = obj.GetOrAddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new(1600, 900);

        obj.GetOrAddComponent<GraphicRaycaster>();

        if (!sortOrder.HasValue)
        {
            canvas.sortingOrder = _popUpOrder;
            _popUpOrder++;
        }
        else
            canvas.sortingOrder = sortOrder.Value;
    }

    public void SetEventSystem(GameObject obj)
    {
        obj.GetOrAddComponent<EventSystem>();
        obj.GetOrAddComponent<InputSystemUIInputModule>();
    }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        SetCanvas(Root);
        SetEventSystem(gameObject);

        return true;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject obj = ResourceManager.Instance.Load<GameObject>($"{name}.prefab");
        obj = Instantiate(obj, Root.transform);

        _sceneUI = obj.GetOrAddComponent<T>();
        return _sceneUI as T;
    }

    public T ShowPopUpUI<T>(string name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject obj = ResourceManager.Instance.Load<GameObject>($"{name}.prefab");
        obj = Instantiate(obj, Root.transform);

        T popUp = obj.GetOrAddComponent<T>();
        _popUpStack.Push(popUp);

        //RefreshTimeScale();

        return popUp;
    }

    public void ClosePopUp(UI_PopUp popUp)
    {
        if (_popUpStack.Count == 0) return;
        if (_popUpStack.Peek() != popUp)
        {
            Debug.LogError($"[UIManager] ClosePopUp({popUp.name}): Close pop up failed");
            return;
        }
        ClosePopUp();
    }

    public void ClosePopUp()
    {
        if (_popUpStack.Count == 0) return;

        UI_PopUp popUp = _popUpStack.Pop();
        ResourceManager.Instance.Destroy(popUp.gameObject);
        _popUpOrder--;
        //RefreshTimeScale();
    }

    public void CloseAllPopUp()
    {
        while (_popUpStack.Count > 0)
            ClosePopUp();
    }
}