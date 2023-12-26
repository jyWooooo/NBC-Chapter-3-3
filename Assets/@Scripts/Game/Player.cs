using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    private bool _initialized = false;
    private Transform _modelRoot;

    public readonly int AnimatorHash_MoveVelocity = Animator.StringToHash("MoveVelocity");

    public Animator Animator { get; private set; }
    public PlayerInputReceiver InputReceiver { get; private set; }
    public RuntimeAnimatorController RuntimeAnimatorController { get; private set; }
    public GameObject Model { get; private set; }

    private void Start()
    {
        Initialize();
    }

    public bool Initialize()
    {
        if (_initialized) return false;

        Animator = GetComponentInChildren<Animator>();
        _modelRoot = transform.GetChild(0);
        InputReceiver = GetComponent<PlayerInputReceiver>();

        _initialized = true;
        return true;
    }

    public void SetInfo(GameObject model, RuntimeAnimatorController anim)
    {
        if (!Initialize()) return;

        Animator = Instantiate(model, _modelRoot).GetComponentInChildren<Animator>();
        Animator.runtimeAnimatorController = anim;
    }
}