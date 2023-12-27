using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    private bool _initialized = false;
    private GameObject _playerCheck;

    public readonly int AnimatorHash_MoveVelocity = Animator.StringToHash("MoveVelocity");
    public readonly int AnimatorHash_IsCharge = Animator.StringToHash("IsCharge");

    public Animator Animator { get; private set; }
    public PlayerInputReceiver InputReceiver { get; private set; }
    public GameObject Model { get; private set; }

    private void Start()
    {
        Initialize();
    }

    public bool Initialize()
    {
        if (_initialized) return false;

        Animator = GetComponentInChildren<Animator>();
        _playerCheck = transform.Find("PlayerCheck").gameObject;
        if (!photonView.IsMine)
            _playerCheck.SetActive(false);
        InputReceiver = GetComponent<PlayerInputReceiver>();

        _initialized = true;
        return true;
    }
}