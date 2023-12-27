using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReceiver : MonoBehaviour
{
    private PlayerInput _input;
    private InputAction _move;
    private InputAction _kick;
    private InputAction _look;

    public PlayerInput Input
    {
        get
        {
            if (_input == null)
                _input = GetComponent<PlayerInput>();
            return _input;
        }
    }

    public InputAction Move
    {
        get
        {
            if (_move == null)
                _move = Input.currentActionMap.FindAction("Move");
            return _move;
        }
    }

    public InputAction Kick
    {
        get
        {
            if (_kick == null)
                _kick = Input.currentActionMap.FindAction("Kick");
            return _kick;
        }
    }

    public InputAction Look
    {
        get
        {
            if (_look == null)
                _look = Input.currentActionMap.FindAction("Look");
            return _look;
        }
    }

    public void BindEvent(InputAction action, InputActionPhase phase, Action<InputAction.CallbackContext> callback)
    {
        if (phase == InputActionPhase.Started)
            action.started += callback;
        else if (phase == InputActionPhase.Performed)
            action.performed += callback;
        else if (phase == InputActionPhase.Canceled)
            action.canceled += callback;
        else
            Debug.LogError("Started, Performed, Canceled 페이즈에만 이벤트를 등록할 수 있습니다.");
    }
}