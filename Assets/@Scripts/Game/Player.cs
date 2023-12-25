using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    private bool _initialized = false;
    private Animator _animator;
    private Transform _modelRoot;

    public RuntimeAnimatorController RuntimeAnimatorController { get; private set; }
    public GameObject Model { get; private set; }

    private void Start()
    {
        Initialize();
    }

    public bool Initialize()
    {
        if (_initialized) return false;

        _animator = GetComponentInChildren<Animator>();
        _modelRoot = transform.GetChild(0);

        _initialized = true;
        return true;
    }

    public void SetInfo(GameObject model, RuntimeAnimatorController anim)
    {
        if (!Initialize()) return;

        _animator = Instantiate(model, _modelRoot).GetComponentInChildren<Animator>();
        _animator.runtimeAnimatorController = anim;
    }
}