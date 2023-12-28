using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun, IHitable
{
    private bool _initialized = false;
    private GameObject _playerCheck;
    private float _remainInvincibleTime;
    private float _invincibleFlashLapse;
    public SkinnedMeshRenderer[] _meshRenderers;

    public readonly int AnimatorHash_MoveVelocity = Animator.StringToHash("MoveVelocity");
    public readonly int AnimatorHash_IsCharge = Animator.StringToHash("IsCharge");

    public PlayerData PlayerData { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerInputReceiver InputReceiver { get; private set; }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        photonView.RPC(nameof(ShowInvincible), RpcTarget.All);
    }

    public bool Initialize()
    {
        if (_initialized) return false;

        Animator = GetComponentInChildren<Animator>();
        _playerCheck = transform.Find("PlayerCheck").gameObject;
        if (!photonView.IsMine)
            _playerCheck.SetActive(false);
        InputReceiver = GetComponent<PlayerInputReceiver>();
        _meshRenderers = transform.Find("Model").gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        PlayerData = ResourceManager.Instance.GetCache<PlayerData>("PlayerData.data");

        _initialized = true;
        return true;
    }

    [PunRPC]
    public void OnHit()
    {
        if (_remainInvincibleTime > 0f)
            return;

        // 府胶迄 扑诀 剁快扁
        if (photonView.IsMine)
            UIManager.Instance.ShowPopUpUI<RespawnPopUpUI>();

        // Character Disable
        gameObject.SetActive(false);

        // ragdoll 积己
        var ragdollResource = ResourceManager.Instance.GetCache<GameObject>("Ragdoll.prefab");
        var ragdoll = Instantiate(ragdollResource, transform.position, transform.rotation);
        ragdoll.GetComponent<Ragdoll>().RandomAddForce(GameManager.Instance.ballState);
    }

    public bool CheckKickerID(int kickerID)
    {
        return photonView.ViewID == kickerID;
    }

    [PunRPC]
    public void SetInvincible()
    {
        _remainInvincibleTime = PlayerData.InvincibleTime;
    }

    [PunRPC]
    private void ShowInvincible()
    {
        if (_remainInvincibleTime < 0)
        {
            if (!_meshRenderers[0].enabled)
                SetActiveMeshRenderers(true);
            return;
        }

        _remainInvincibleTime -= Time.fixedDeltaTime;
        _invincibleFlashLapse += Time.fixedDeltaTime;
        if (PlayerData.InvincibleFlashInterval < _invincibleFlashLapse)
        {
            _invincibleFlashLapse = 0f;
            SetActiveMeshRenderers(!_meshRenderers[0].enabled);
        }
    }

    [PunRPC]
    private void Respawn()
    {
        gameObject.SetActive(true);
        SetInvincible();
    }

    private void SetActiveMeshRenderers(bool active)
    {
        foreach (var renderer in _meshRenderers)
            renderer.enabled = active;
    }
}