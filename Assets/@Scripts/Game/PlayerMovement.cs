using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rigid;
    private float _moveSpeed = 3.0f;
    private PhotonView _pv;

    public Vector2 Movement { get; private set; }


    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        _rigid = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
        _player.Initialize();
        _player.InputReceiver.BindEvent(_player.InputReceiver.Move, InputActionPhase.Performed, ReadMovement);
        _player.InputReceiver.BindEvent(_player.InputReceiver.Move, InputActionPhase.Canceled, ReadMovement);
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void ReadMovement(InputAction.CallbackContext context)
    {
        if (!_pv.IsMine)
            return;

        Movement = context.ReadValue<Vector2>();
        _player.Animator.SetFloat(_player.AnimatorHash_MoveVelocity, Movement.magnitude);
    }

    public void Move()
    {
        if (Mathf.Approximately(Movement.magnitude, 0f) || !_pv.IsMine)
            return;

        transform.forward = new Vector3(Movement.x, 0, Movement.y);
        _rigid.MovePosition(_rigid.position + _moveSpeed * Time.fixedDeltaTime * transform.forward);
    }
}