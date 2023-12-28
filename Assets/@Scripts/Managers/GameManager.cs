using Photon.Pun;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject Ball {  get; private set; }
    public GameObject MyPlayer { get; private set; }
    private Rigidbody _ballRigid;

    public struct BallStateStruct
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 angularVelocity;
    }

    // �� ����ü�� �̿��� ���� ������ ��� ������ ���
    public BallStateStruct ballState = new()
    {
        position = new Vector3(0, 10, 0),
        rotation = Quaternion.identity,
        velocity = Vector3.zero,
        angularVelocity = Vector3.zero,
    };

    protected override void Start()
    {
        base.Start();

        // 1. ���ҽ� �ε�
        ResourceLoad((key, count, total) =>
        {
            if (count == total)
            {
                // 2. ���� ���� ����
                PhotonConnect(() =>
                {
                    // 3. ��, ĳ����, �� ���� (���� ����)
                    GameStart();
                });
            }
        });
    }

    private void Update()
    {
        if (Ball == null)
        {
            Ball = GameObject.FindWithTag("Ball");
            if (Ball != null)
                _ballRigid = Ball.GetComponent<Rigidbody>();
            return;
        }

        // �� ���� ����
        ballState.position = Ball.transform.position;
        ballState.rotation = Ball.transform.rotation;
        ballState.velocity = _ballRigid.velocity;
        ballState.angularVelocity = _ballRigid.angularVelocity;
    }

    public void PhotonConnect(Action callback = null)
    {
        PhotonConnector.Instance.OnComplete += callback;
        PhotonConnector.Instance.ConnectNow();
    }

    public void ResourceLoad(Action<string, int, int> callback = null)
    {
        ResourceManager.Instance.LoadAllAsync<UnityEngine.Object>("Game", callback);
    }

    public void GameStart()
    {
        // Addressable���� Load�� prefab�� PhotonNetwork.PrefabPool�� ����ؾ���.
        SetPhotonPrefabPool();

        CreateMap();
        PlayerSpawn();
        BallSpawn();

        Camera.main.clearFlags = CameraClearFlags.Skybox;
    }

    public void BallSpawn()
    {
        // Host������ �� ����
        if (PhotonNetwork.IsMasterClient)
        {
            Ball = PhotonNetwork.Instantiate("Ball.prefab", ballState.position, ballState.rotation);
            _ballRigid = Ball.GetComponent<Rigidbody>();
            _ballRigid.velocity = ballState.velocity;
            _ballRigid.angularVelocity = ballState.angularVelocity;
        }
    }

    private void CreateMap()
    {
        var floorPrefab = ResourceManager.Instance.GetCache<GameObject>("Floor.prefab");
        Instantiate(floorPrefab);
    }

    private void SetPhotonPrefabPool()
    {
        var pool = PhotonNetwork.PrefabPool as DefaultPool;

        // 1. �÷��̾� prefab ���
        var playerPrefab = ResourceManager.Instance.GetCache<GameObject>("Player.prefab");
        pool.ResourceCache.Add("Player.prefab", playerPrefab);

        // 2. Ball prefab ���
        var ballPrefab = ResourceManager.Instance.GetCache<GameObject>("Ball.prefab");
        pool.ResourceCache.Add("Ball.prefab", ballPrefab);

        //// 3. Model fbx ���
        //var modelFbx = ResourceManager.Instance.GetCache<GameObject>("XBot.fbx");
        //pool.ResourceCache.Add("XBot.fbx", modelFbx);
    }

    public void PlayerSpawn()
    {
        // PhotonNetwork.PrefabPool�� �̿��� ����
        MyPlayer = PhotonNetwork.Instantiate("Player.prefab", Vector3.zero, Quaternion.identity);
        MyPlayer.GetComponent<Player>().photonView.RPC("SetInvincible", RpcTarget.All);
    }
}