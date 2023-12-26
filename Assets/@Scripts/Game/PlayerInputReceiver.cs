using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReceiver : MonoBehaviour
{
    private PlayerInput _input;
    private InputAction _move;

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

    public void BindEvent(InputAction action, InputActionPhase phase, Action<InputAction.CallbackContext> callback)
    {
        if (phase == InputActionPhase.Started)
            action.started += callback;
        else if (phase == InputActionPhase.Performed)
            action.performed += callback;
        else if (phase == InputActionPhase.Canceled)
            action.canceled += callback;
        else
            Debug.LogError("Started, Performed, Canceled ������� �̺�Ʈ�� ����� �� �ֽ��ϴ�.");
    }
}