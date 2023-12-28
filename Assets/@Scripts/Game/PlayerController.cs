using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rigid;
    private PhotonView _pv;
    private bool _isCharge;
    private float _chagedPower;
    private Vector2 _moveVelocity;
    private Vector3 _kickDirection;
    private float _remainKickCooltime;
    private Ball _ball;
    private PlayerData _playerData;


    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        _rigid = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
        _player.Initialize();
        _playerData = _player.PlayerData;
        if (_pv.IsMine)
        {
            var receiver = _player.InputReceiver;

            receiver.BindEvent(receiver.Move, InputActionPhase.Performed, ReadMovement);
            receiver.BindEvent(receiver.Move, InputActionPhase.Canceled, ReadMovement);
            receiver.BindEvent(receiver.Kick, InputActionPhase.Started, KickChargeStart);
            receiver.BindEvent(receiver.Kick, InputActionPhase.Canceled, Kick);
            receiver.BindEvent(receiver.Look, InputActionPhase.Performed, Look);
        }
    }

    private void FixedUpdate()
    {
        if (!_pv.IsMine)
            return;

        Move();
        KickCharge();
        _remainKickCooltime -= Time.fixedDeltaTime;
    }

    public void ReadMovement(InputAction.CallbackContext context)
    {
        _moveVelocity = context.ReadValue<Vector2>();

        if (_isCharge)
            return;
        _player.Animator.SetFloat(_player.AnimatorHash_MoveVelocity, _moveVelocity.magnitude);
    }

    public void Move()
    {
        if (Mathf.Approximately(_moveVelocity.magnitude, 0f) || _isCharge)
            return;

        transform.forward = new Vector3(_moveVelocity.x, 0, _moveVelocity.y);
        _rigid.MovePosition(_rigid.position + _playerData.MoveSpeed * Time.fixedDeltaTime * transform.forward);
    }

    public void KickChargeStart(InputAction.CallbackContext context)
    {
        if (_remainKickCooltime > 0f)
            return;

        DetectBall();
        if (_ball == null)
            return;

        _ball.photonView.RPC("Catch", RpcTarget.All, transform.position, _pv.ViewID);
        _isCharge = true;
        _player.Animator.SetFloat(_player.AnimatorHash_MoveVelocity, 0f);
        _player.Animator.SetBool(_player.AnimatorHash_IsCharge, true);
    }

    public void KickCharge()
    {
        if (!_isCharge)
            return;

        _chagedPower += Time.fixedDeltaTime;
        transform.forward = _kickDirection;
        if (_chagedPower > _playerData.MaxPower)
            Kick();
    }

    public void Kick(InputAction.CallbackContext context) => Kick();

    public void Kick()
    {
        if (_ball == null)
            return;
        _ball.photonView.RPC("Shoot", RpcTarget.All, _kickDirection, _chagedPower, _pv.ViewID);
        _isCharge = false;
        _player.Animator.SetFloat(_player.AnimatorHash_MoveVelocity, _moveVelocity.magnitude);
        _player.Animator.SetBool(_player.AnimatorHash_IsCharge, false);
        _ball = null;
        _chagedPower = 0f;
        _remainKickCooltime = _playerData.KickCooltime;
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector3 pos = context.ReadValue<Vector2>();
        var ray = Camera.main.ScreenPointToRay(pos);
        Physics.Raycast(ray, out var info, float.PositiveInfinity, 0b1000);
        pos = info.point;
        _kickDirection = pos - transform.position;
        _kickDirection.y = 0;
        _kickDirection.Normalize();
    }

    private void DetectBall()
    {
        if (GameManager.Instance.Ball == null)
            return;

        var ball = GameManager.Instance.Ball;
        float dist = Vector3.Distance(ball.transform.position, transform.position);
        if (dist < _playerData.BallDetectRange)
            _ball = ball.GetComponent<Ball>();
        else
            _ball = null;
    }
}